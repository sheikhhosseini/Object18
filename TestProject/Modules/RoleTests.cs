using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Modules.RoleModule.Dtos;
using Core.Modules.RoleModule.Services;
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
public class RoleTests : DbContextFixture
{
    private readonly IMapper _mapper;

    public RoleTests(AutoMapperFixture mapperFixture)
    {
        _mapper = mapperFixture.Mapper;
    }

    private IRoleService CreateService(MainDbContext context)
    {
        return new RoleService(context, _mapper, new DataTableService(_mapper));
    }

    /// <summary>
    /// ایجاد نقش باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task Create_Role_ReturnsSuccess()
    {
        // Arrange
        var permission1 = PermissionFactory.Create();
        var permission2 = PermissionFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddRangeAsync(permission1, permission2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var createDto = new RoleCreateDto
            {
                Name = "Role1",
                Description = RandomFactory.Text(10),
                PermissionIds = new List<long>{ permission1.Id }
            };

            // Act
            var result = await service.Create(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Success);
            result.Type.Should().Be(OperationResultType.Single);

            var roleList = dbContext.Set<Role>().ToList();
            roleList.Should().HaveCount(1);
            Role role = roleList.First();
            role.Id.Should().BeGreaterThan(0);
            role.ConcurrencyStamp.Should().NotBeNullOrWhiteSpace();
            role.CreateDate.Should().NotBe(null);
            role.LastUpdateDate.Should().Be(role.CreateDate);
            role.Description.Should().Be(createDto.Description);
            role.RolePermissions.Should().HaveCount(1).And
                .OnlyContain(r => r.PermissionId == permission1.Id);
        }
    }

    /// <summary>
    /// ایجاد نقش باید خطا بدهد
    /// اگر نام تکراری باشد 
    /// </summary>
    [Fact]
    public async Task Create_Role_Duplicate_Name_ReturnsFailed()
    {
        // Arrange  
        var savedRole = RoleFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedRole);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var createDto = new RoleCreateDto
            {
                Name = savedRole.Name,
                Description = RandomFactory.Text(10),
                PermissionIds = savedRole.RolePermissions.Select(x=> x.PermissionId).ToList()
            };

            // Act
            var result = await service.Create(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Failed);
            result.Type.Should().Be(OperationResultType.Single);
            result.Message.Should().Contain("نام");

            dbContext.Set<Role>().Should().HaveCount(1);
        }
    }

    /// <summary>
    /// ویرایش نقش باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task Update_Role_ReturnsSuccess()
    {
        // Arrange  
        var permission1 = PermissionFactory.Create();
        var permission2 = PermissionFactory.Create();

        var savedRole = RoleFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedRole);
            await dbContext.AddRangeAsync(permission1, permission2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var updateDto = new RoleUpdateDto
            {
                Id = savedRole.Id,
                ConcurrencyStamp = savedRole.ConcurrencyStamp,
                Name = "New Role",
                Description = RandomFactory.Text(10),
                PermissionIds = new List<long> { permission1.Id, permission2.Id }
            };

            // Act
            var result = await service.Update(updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Success);
            result.Type.Should().Be(OperationResultType.Single);

            var roleList = dbContext.Set<Role>().ToList();
            roleList.Should().HaveCount(1);
            Role role = roleList.First();
            role.Id.Should().BeGreaterThan(0);
            role.ConcurrencyStamp.Should().NotBeNullOrWhiteSpace();
            role.CreateDate.Should().NotBe(null);
            role.LastUpdateDate.Should().Be(role.CreateDate);
            role.Description.Should().Be(updateDto.Description);
            role.RolePermissions.Should().HaveCount(2).And
                .OnlyContain(r => r.PermissionId == permission1.Id || r.PermissionId == permission2.Id);
        }
    }

    /// <summary>
    /// ویرایش نقش باید خطا بدهد
    /// اگر شماه نام باشد 
    /// </summary>
    [Fact]
    public async Task Update_Role_Duplicate_Name_ReturnsFailed()
    {
        // Arrange  
        var savedRole1 = RoleFactory.Create();
        var savedRole2 = RoleFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntitiesAsync(savedRole1, savedRole2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var updateDto = new RoleUpdateDto
            {
                Id = savedRole1.Id,
                ConcurrencyStamp = savedRole1.ConcurrencyStamp,
                Name = savedRole2.Name,
                Description = RandomFactory.Text(10),
                PermissionIds = savedRole1.RolePermissions.Select(x => x.PermissionId).ToList()
            };

            // Act
            var result = await service.Update(updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Failed);
            result.Type.Should().Be(OperationResultType.Single);
            result.Message.Should().Contain("نام");

            dbContext.Set<Role>().Should().HaveCount(2);
        }
    }

    /// <summary>
    /// ویرایش نقش باید خطا بدهد
    /// اگر کانکارنسی معتبر نباشد 
    /// </summary>
    [Fact]
    public async Task Update_Role_Wrong_ConcurrencyStamp_ReturnsFailed()
    {
        // Arrange  
        var savedRole = RoleFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedRole);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);
            var updateDto = new RoleUpdateDto
            {
                Id = savedRole.Id,
                ConcurrencyStamp = savedRole.ConcurrencyStamp,
                Name = "New Role",
                Description = RandomFactory.Text(10),
                PermissionIds = savedRole.RolePermissions.Select(x => x.PermissionId).ToList()
            };

            await service.Update(updateDto);
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var updateDto = new RoleUpdateDto
            {
                Id = savedRole.Id,
                ConcurrencyStamp = savedRole.ConcurrencyStamp,
                Name = "New Role 2",
                Description = RandomFactory.Text(10),
                PermissionIds = savedRole.RolePermissions.Select(x => x.PermissionId).ToList()

            };

            // Act
            Func<Task> result = async () => { await service.Update(updateDto); };

            // Assert
            await result.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }

    /// <summary>
    /// حذف نقش باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task Delete_Role_ReturnsSuccess()
    {
        // Arrange  
        var savedRole1 = RoleFactory.Create();
        var savedRole2 = RoleFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntitiesAsync(savedRole1, savedRole2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var deleteDtos = new List<RoleDeleteDto>
            {
                new()
                {
                    Id = savedRole1.Id,
                    ConcurrencyStamp = savedRole1.ConcurrencyStamp,
                },
                new()
                {
                    Id = savedRole2.Id,
                    ConcurrencyStamp = savedRole2.ConcurrencyStamp,
                }
            };

            // Act
            var result = await service.Delete(deleteDtos);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().Be(Response.Success);
            result.Type.Should().Be(OperationResultType.Single);

            dbContext.Set<Role>().Should().HaveCount(0);
        }
    }

    /// <summary>
    /// حذف نقش باید خطا بدخد اگر کانکارنسی اشتباه باشد   
    /// </summary>
    [Fact]
    public async Task Delete_Role_Wrong_ConcurrencyStamp_ReturnsFailed()
    {
        // Arrange  
        var savedRole = RoleFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            await dbContext.AddEntityAsync(savedRole);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            var service = CreateService(dbContext);

            var deleteDtos = new List<RoleDeleteDto>
            {
                new()
                {
                    Id = savedRole.Id,
                    ConcurrencyStamp = RandomFactory.Text(10)
                }
            };

            // Act
            Func<Task> result = async () => { await service.Delete(deleteDtos); };

            // Assert
            await result.Should().ThrowAsync<DbUpdateConcurrencyException>();

            dbContext.Set<Role>().Should().HaveCount(1);
        }
    }
}