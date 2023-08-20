using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Modules.UserModule.Dtos;
using Core.Modules.UserModule.Services;
using Core.Shared.DataTable;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using FluentAssertions;
using TestProject.Base;
using TestProject.MyRandomGenerator;
using Xunit;

namespace TestProject.Modules;

[Collection(nameof(AllTestsCollectionFixture))]
public class UserTests : DbContextFixture
{
    private readonly IMapper _mapper;

    public UserTests(AutoMapperFixture mapperFixture)
    {
        _mapper = mapperFixture.Mapper;
    }

    private IUserService CreateService(MainDbContext context)
    {
        return new UserService(context, _mapper, new DataTableService(_mapper));
    }

    /// <summary>
    /// ایجاد کاربر باید موفقیت آمیز باشد   
    /// </summary>
    [Fact]
    public async Task AddUser()
    {
        // Arrange
        await using var dbContext = CreateNewDbContext();
        var service = CreateService(dbContext);

        var createDto = new UserCreateDto
        {
            FirstName = RandomFactory.FirstName(),
            LastName = RandomFactory.LastName(),
            //Password = RandomFactory.Text(6),
            Email = RandomFactory.Email(),
            //ActiveCode = RandomFactory.Text(10),
            //UserImage = null,
            MobileNumber = "09131111111",
            //UserRoles = null
        };

        // Act
        var result = await service.Create(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Response.Should().Be(Response.Success);
        result.Type.Should().Be(OperationResultType.Single);

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
        user.MobileNumber.Should().Be(createDto.MobileNumber);
    }
}