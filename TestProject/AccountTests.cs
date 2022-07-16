using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Modules.Account.Dtos;
using Core.Modules.Account.Results;
using Core.Modules.Account.Services;
using Data.Context;
using Data.Models;
using FluentAssertions;
using TestProject.Base;
using TestProject.Factory;
using TestProject.MyRandomGenerator;
using Xunit;

namespace TestProject;

[Collection(nameof(AllTestsCollectionFixture))]
public class AccountTests : DbContextFixture
{
    private readonly IMapper _mapper;

    public AccountTests(AutoMapperFixture mapperFixture)
    {
        _mapper = mapperFixture.Mapper;
    }

    private IAccountService CreateService(MainDbContext context)
    {
        return new AccountService(context, _mapper, null, null);
    }

    /// <summary>
    /// ثبت نام کاربر باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task RegisterUser()
    {
        // Arrange
        await using var dbContext = CreateNewDbContext();
        //var repository = new GenericRepository<User>(dbContext);
        var service = CreateService(dbContext);

        var createDto = new RegisterDto
        {
            Email = RandomFactory.Email(),
            FirstName = RandomFactory.FirstName(),
            LastName = RandomFactory.LastName(),
            Password = RandomFactory.Text(5)
        };

        // Act
        await service.RegisterUser(createDto, false);

        // Assert
        List<User> userList = dbContext.Set<User>().ToList();
        userList.Should().HaveCount(1);
        User user = userList.First();
        user.Id.Should().BeGreaterThan(0);
        user.FirstName.Should().Be(createDto.FirstName);
        user.LastName.Should().Be(createDto.LastName);
        user.Email.Should().Be(createDto.Email);
        user.Password.Should().NotBeNullOrEmpty();
        user.ActiveCode.Should().NotBeNullOrEmpty();
        user.IsActive.Should().BeFalse();
        user.CreateDate.Should().NotBe(null);
        user.LastUpdateDate.Should().Be(user.CreateDate);
        user.IsDelete.Should().BeFalse();
        user.UserImage.Should().Be("Default.jpg");
        user.MobileNumber.Should().BeNull();
    }

    /// <summary>
    /// حساب کاربری کاربر باید با موفقیت فعال شود
    /// اگر حساب غیر فعال باشد
    /// و لینک فعالسازی معتبر باشد
    /// </summary>
    [Fact]
    public async Task Active_User_Account_Success_Result()
    {
        // Arrange
        User user = UserFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            //var repository = new GenericRepository<User>(dbContext);
            var service = CreateService(dbContext);

            string activeCode = user.ActiveCode;

            // Act
            var result = await service.ActiveAccount(activeCode);

            // Assert
            result.Should().Be(ActiveAccountResult.Success);

            List<User> userList = dbContext.Set<User>().ToList();
            userList.Should().HaveCount(1);
            User updatedUser = userList.First();
            updatedUser.Id.Should().BeGreaterThan(0);
            updatedUser.FirstName.Should().Be(user.FirstName);
            updatedUser.LastName.Should().Be(user.LastName);
            updatedUser.Email.Should().Be(user.Email);
            updatedUser.Password.Should().NotBeNullOrEmpty();
            updatedUser.ActiveCode.Should().NotBeNullOrEmpty().And.NotBe(user.ActiveCode);
            updatedUser.IsActive.Should().BeTrue();
            updatedUser.CreateDate.Should().Be(user.CreateDate);
            updatedUser.LastUpdateDate.Should().NotBe(user.LastUpdateDate);
            updatedUser.IsDelete.Should().BeFalse();
            updatedUser.UserImage.Should().Be("Default.jpg");
            updatedUser.MobileNumber.Should().BeNull();
        }
    }

    /// <summary>
    /// حساب کاربری کاربر نباید فعال شود
    /// اگر لینک فعالسازی نامعتبر باشد
    /// </summary>
    [Fact]
    public async Task Active_User_Account_Failed_Result()
    {
        // Arrange
        User user = UserFactory.Create();

        await using (var dbContext = CreateNewDbContext())
        {
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            //var repository = new GenericRepository<User>(dbContext);
            var service = CreateService(dbContext);

            string activeCode = RandomFactory.Text(15);

            // Act
            var result = await service.ActiveAccount(activeCode);

            // Assert
            result.Should().Be(ActiveAccountResult.Failed);

            List<User> userList = dbContext.Set<User>().ToList();
            userList.Should().HaveCount(1);
            User updatedUser = userList.First();
            updatedUser.Id.Should().BeGreaterThan(0);
            updatedUser.FirstName.Should().Be(user.FirstName);
            updatedUser.LastName.Should().Be(user.LastName);
            updatedUser.Email.Should().Be(user.Email);
            updatedUser.Password.Should().NotBeNullOrEmpty();
            updatedUser.ActiveCode.Should().NotBeNullOrEmpty().And.Be(user.ActiveCode);
            updatedUser.IsActive.Should().Be(user.IsActive);
            updatedUser.CreateDate.Should().Be(user.CreateDate);
            updatedUser.LastUpdateDate.Should().Be(user.LastUpdateDate);
            updatedUser.IsDelete.Should().BeFalse();
            updatedUser.UserImage.Should().Be("Default.jpg");
            updatedUser.MobileNumber.Should().BeNull();
        }
    }

    /// <summary>
    /// حساب کاربری کاربر نباید فعال شود
    /// اگر قبلا فعال شده
    /// </summary>
    [Fact]
    public async Task Active_User_Account_AlreadyActive_Result()
    {
        // Arrange
        User user = UserFactory.Create();
        user.IsActive = true;

        await using (var dbContext = CreateNewDbContext())
        {
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = CreateNewDbContext())
        {
            //var repository = new GenericRepository<User>(dbContext);
            var service = CreateService(dbContext);

            string activeCode = user.ActiveCode;

            // Act
            var result = await service.ActiveAccount(activeCode);

            // Assert
            result.Should().Be(ActiveAccountResult.AlreadyActive);

            List<User> userList = dbContext.Set<User>().ToList();
            userList.Should().HaveCount(1);
            User updatedUser = userList.First();
            updatedUser.Id.Should().BeGreaterThan(0);
            updatedUser.FirstName.Should().Be(user.FirstName);
            updatedUser.LastName.Should().Be(user.LastName);
            updatedUser.Email.Should().Be(user.Email);
            updatedUser.Password.Should().NotBeNullOrEmpty();
            updatedUser.ActiveCode.Should().NotBeNullOrEmpty().And.Be(user.ActiveCode);
            updatedUser.IsActive.Should().BeTrue();
            updatedUser.CreateDate.Should().Be(user.CreateDate);
            updatedUser.LastUpdateDate.Should().Be(user.LastUpdateDate);
            updatedUser.IsDelete.Should().BeFalse();
            updatedUser.UserImage.Should().Be("Default.jpg");
            updatedUser.MobileNumber.Should().BeNull();
        }
    }
}