using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Modules.MemberModule.Dtos;
using Core.Modules.MemberModule.Services;
using Core.Shared.DataTable;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestProject.Base;
using TestProject.Factory;
using TestProject.MyRandomGenerator;
using Xunit;

namespace TestProject.Modules;

[Collection(nameof(AllTestsCollectionFixture))]
public class MemberTests : DbContextFixture
{
    private readonly IMapper _mapper;

    public MemberTests(AutoMapperFixture mapperFixture)
    {
        _mapper = mapperFixture.Mapper;
    }

    private IMemberService CreateService(MainDbContext context)
    {
        return new MemberService(context, _mapper, new DataTableService(_mapper));
    }

    /// <summary>
    /// ایجاد عضو باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task Create_Member_ReturnsSuccess()
    {
        // Arrange
        await using var dbContext = CreateNewDbContext();
        var service = CreateService(dbContext);

        var createDto = new MemberCreateDto
        {
            FirstName = RandomFactory.FirstName(),
            LastName = RandomFactory.LastName(),
            Email = RandomFactory.Email(),
            MobileNumber = "09131111111",
            KodMeli = "1271452369",
            Address = RandomFactory.Text(20),
            Gender = true,
            DateOfBirth = "1378/01/01",
            RegisterDate = "1402/01/01"
        };

        // Act
        var result = await service.Create(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Response.Should().Be(Response.Success);
        result.Type.Should().Be(OperationResultType.Single);

        var memberList = dbContext.Set<Member>().ToList();
        memberList.Should().HaveCount(1);
        Member member = memberList.First();
        member.Id.Should().BeGreaterThan(0);
        member.ConcurrencyStamp.Should().NotBeNullOrWhiteSpace();
        member.FirstName.Should().Be(createDto.FirstName);
        member.LastName.Should().Be(createDto.LastName);
        member.Email.Should().Be(createDto.Email);
        member.Gender.Should().Be(createDto.Gender);
        member.DateOfBirth.Should().Be(createDto.DateOfBirth);
        member.RegisterDate.Should().Be(createDto.RegisterDate);
        member.CreateDate.Should().NotBe(null);
        member.LastUpdateDate.Should().Be(member.CreateDate);
        member.IsDelete.Should().BeFalse();
        member.Image.Should().BeNullOrEmpty();
        member.MobileNumber.Should().Be(createDto.MobileNumber);
    }

    /// <summary>
    /// ایجاد عضو باید خطا بدهد
    /// اگر شماه موبایل تکراری باشد 
    /// </summary>
    [Fact]
    public async Task Create_Member_Duplicate_MobileNumber_ReturnsFailed()
    {
        // Arrange  
        var savedMember = MemberFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedMember);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var createDto = new MemberCreateDto
            {
                FirstName = RandomFactory.FirstName(),
                LastName = RandomFactory.LastName(),
                Email = RandomFactory.Email(),
                MobileNumber = savedMember.MobileNumber,
                KodMeli = "1271452369",
                Address = RandomFactory.Text(20),
                Gender = true,
                DateOfBirth = "1378/01/01",
                RegisterDate = "1402/01/01"
            };

            // Act
            var result = await service.Create(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Failed);
            result.Type.Should().Be(OperationResultType.Single);
            result.Message.Should().Contain("شماره تلفن");

            dbContext.Set<Member>().Should().HaveCount(1);
        }
    }

    /// <summary>
    /// ایجاد عضو باید خطا بدهد
    /// اگر کد ملی تکراری باشد 
    /// </summary>
    [Fact]
    public async Task Create_Member_Duplicate_KodMeli_ReturnsFailed()
    {
        // Arrange  
        var savedMember = MemberFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedMember);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var createDto = new MemberCreateDto
            {
                FirstName = RandomFactory.FirstName(),
                LastName = RandomFactory.LastName(),
                Email = RandomFactory.Email(),
                MobileNumber = "09132145698",
                KodMeli = savedMember.KodMeli,
                Address = RandomFactory.Text(20),
                Gender = true,
                DateOfBirth = "1378/01/01",
                RegisterDate = "1402/01/01"
            };

            // Act
            var result = await service.Create(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Failed);
            result.Type.Should().Be(OperationResultType.Single);
            result.Message.Should().Contain("کد ملی");

            dbContext.Set<Member>().Should().HaveCount(1);
        }
    }

    /// <summary>
    /// ویرایش عضو باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task Update_Member_ReturnsSuccess()
    {
        // Arrange  
        var savedMember = MemberFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedMember);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var updateDto = new MemberUpdateDto
            {
                Id = savedMember.Id,
                ConcurrencyStamp = savedMember.ConcurrencyStamp,
                FirstName = RandomFactory.FirstName(),
                LastName = RandomFactory.LastName(),
                Email = RandomFactory.Email(),
                MobileNumber = savedMember.MobileNumber,
                KodMeli = "1271452369",
                Address = RandomFactory.Text(20),
                Gender = true,
                DateOfBirth = "1378/01/01",
                RegisterDate = "1402/01/01"
            };

            // Act
            var result = await service.Update(updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Success);
            result.Type.Should().Be(OperationResultType.Single);

            var memberList = dbContext.Set<Member>().ToList();
            memberList.Should().HaveCount(1);
            Member member = memberList.First();
            member.Id.Should().BeGreaterThan(0);
            member.ConcurrencyStamp.Should().NotBeNullOrWhiteSpace();
            member.FirstName.Should().Be(updateDto.FirstName);
            member.LastName.Should().Be(updateDto.LastName);
            member.Email.Should().Be(updateDto.Email);
            member.Gender.Should().Be(updateDto.Gender);
            member.DateOfBirth.Should().Be(updateDto.DateOfBirth);
            member.RegisterDate.Should().Be(updateDto.RegisterDate);
            member.CreateDate.Should().NotBe(null);
            member.LastUpdateDate.Should().Be(member.LastUpdateDate);
            member.IsDelete.Should().BeFalse();
            member.Image.Should().BeNullOrEmpty();
            member.MobileNumber.Should().Be(updateDto.MobileNumber);
        }
    }

    /// <summary>
    /// ویرایش عضو باید خطا بدهد
    /// اگر شماه موبایل تکراری باشد 
    /// </summary>
    [Fact]
    public async Task Update_Member_Duplicate_MobileNumber_ReturnsFailed()
    {
        // Arrange  
        var savedMember1 = MemberFactory.Create();
        var savedMember2 = MemberFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntitiesAsync(savedMember1, savedMember2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var updateDto = new MemberUpdateDto
            {
                Id = savedMember1.Id,
                ConcurrencyStamp = savedMember1.ConcurrencyStamp,
                FirstName = RandomFactory.FirstName(),
                LastName = RandomFactory.LastName(),
                Email = RandomFactory.Email(),
                MobileNumber = savedMember2.MobileNumber,
                KodMeli = "1271452369",
                Address = RandomFactory.Text(20),
                Gender = true,
                DateOfBirth = "1378/01/01",
                RegisterDate = "1402/01/01"
            };

            // Act
            var result = await service.Update(updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Failed);
            result.Type.Should().Be(OperationResultType.Single);
            result.Message.Should().Contain("شماره تلفن");

            dbContext.Set<Member>().Should().HaveCount(2);
        }
    }

    /// <summary>
    /// ویرایش عضو باید خطا بدهد
    /// اگر شماه کد ملی باشد 
    /// </summary>
    [Fact]
    public async Task Update_Member_Duplicate_KodMeli_ReturnsFailed()
    {
        // Arrange  
        var savedMember1 = MemberFactory.Create();
        var savedMember2 = MemberFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntitiesAsync(savedMember1, savedMember2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var updateDto = new MemberUpdateDto
            {
                Id = savedMember1.Id,
                ConcurrencyStamp = savedMember1.ConcurrencyStamp,
                FirstName = RandomFactory.FirstName(),
                LastName = RandomFactory.LastName(),
                Email = RandomFactory.Email(),
                MobileNumber = "09135412365",
                KodMeli = savedMember2.KodMeli,
                Address = RandomFactory.Text(20),
                Gender = true,
                DateOfBirth = "1378/01/01",
                RegisterDate = "1402/01/01"
            };

            // Act
            var result = await service.Update(updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Failed);
            result.Type.Should().Be(OperationResultType.Single);
            result.Message.Should().Contain("کد ملی");

            dbContext.Set<Member>().Should().HaveCount(2);
        }
    }

    /// <summary>
    /// ویرایش عضو باید خطا بدهد
    /// اگر کانکارنسی معتبر نباشد 
    /// </summary>
    [Fact]
    public async Task Update_Member_Wrong_ConcurrencyStamp_ReturnsFailed()
    {   
        // Arrange  
        var savedMember = MemberFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedMember);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);
            var updateDto = new MemberUpdateDto
            {
                Id = savedMember.Id,
                ConcurrencyStamp = savedMember.ConcurrencyStamp,
                FirstName = RandomFactory.FirstName(),
                LastName = RandomFactory.LastName(),
                Email = RandomFactory.Email(),
                MobileNumber = "09135412365",
                KodMeli = "1272574136",
                Address = RandomFactory.Text(20),
                Gender = true,
                DateOfBirth = "1378/01/01",
                RegisterDate = "1402/01/01"
            };

            await service.Update(updateDto);
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var updateDto = new MemberUpdateDto
            {
                Id = savedMember.Id,
                ConcurrencyStamp = savedMember.ConcurrencyStamp,
                FirstName = RandomFactory.FirstName(),
                LastName = RandomFactory.LastName(),
                Email = RandomFactory.Email(),
                MobileNumber = "09135412365",
                KodMeli = "1272574136",
                Address = RandomFactory.Text(20),
                Gender = true,
                DateOfBirth = "1378/01/01",
                RegisterDate = "1402/01/01"
            };

            // Act
            Func<Task> result = async () => { await service.Update(updateDto); };

            // Assert
            await result.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }

    /// <summary>
    /// حذف عضو باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task Delete_Member_ReturnsSuccess()
    {
        // Arrange  
        var savedMember1 = MemberFactory.Create();
        var savedMember2 = MemberFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntitiesAsync(savedMember1, savedMember2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var deleteDtos = new List<MemberDeleteDto>
            {
                new()
                {
                    Id = savedMember1.Id,
                    ConcurrencyStamp = savedMember1.ConcurrencyStamp,
                },
                new()
                {
                    Id = savedMember2.Id,
                    ConcurrencyStamp = savedMember2.ConcurrencyStamp,
                }
            };

            // Act
            var result = await service.Delete(deleteDtos);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Success);
            result.Type.Should().Be(OperationResultType.Single);

            dbContext.Set<Member>().Should().HaveCount(0);
        }
    }

    /// <summary>
    /// حذف عضو باید خطا بدخد اگر کانکارنسی اشتباه باشد   
    /// </summary>
    [Fact]
    public async Task Delete_Member_Wrong_ConcurrencyStamp_ReturnsFailed()
    {
        // Arrange  
        var savedMember = MemberFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedMember);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var deleteDtos = new List<MemberDeleteDto>
            {
                new()
                {
                    Id = savedMember.Id,
                    ConcurrencyStamp = RandomFactory.Text(10)
                }
            };

            // Act
            Func<Task> result = async () => { await service.Delete(deleteDtos); };

            // Assert
            await result.Should().ThrowAsync<DbUpdateConcurrencyException>();

            dbContext.Set<Member>().Should().HaveCount(1);
        }
    }
}