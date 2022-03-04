using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Modules.Account.Dtos;
using Core.Modules.Account.Services;
using Core.Repository;
using Data.Models;
using FluentAssertions;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using TestProject.Base;
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

    private IAccountService CreateService(GenericRepository<User> context)
    {
        return new AccountService(context, _mapper);
    }

    /// <summary>
    /// ثبت نام کاربر باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task RegisterUser()
    {
        // Arrange
        await using var dbContext = CreateNewDbContext();
        var repository = new GenericRepository<User>(dbContext);
        var service = CreateService(repository);

        var createDto = new RegisterDto
        {
            Email = RandomFactory.Email(),
            FirstName = RandomFactory.FirstName(),
            LastName = RandomFactory.LastName(),
            Password = RandomFactory.Text(5)
        };

        // Act
        await service.RegisterUser(createDto);

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
}