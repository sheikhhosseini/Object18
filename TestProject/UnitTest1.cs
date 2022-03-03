using System.Threading.Tasks;
using AutoMapper;
using Core.Modules.Account.Dtos;
using Core.Modules.Account.Services;
using Core.Repository;
using Data.Context;
using Data.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace TestProject
{
    [Collection(nameof(AllTestsCollectionFixture))]
    public class UnitTest1 : DbContextFixture
    {
        private readonly IMapper _mapper;

        public UnitTest1(AutoMapperFixture mapperFixture)
        {
            _mapper = mapperFixture.Mapper;
        }

        private IAccountService CreateService(GenericRepository<User> context)
        {
            return new AccountService(context, _mapper);
        }



        [Fact]
        public async Task Test1()
        {
            await using (var context = CreateNewDbContext())
            {


                var repo = new GenericRepository<User>(context);

                // Arrange
                var service = CreateService(repo);

                var createDto = new RegisterDto()
                {
                    Email = "a@a.com",
                    FirstName = "ali",
                    LastName = "saeedi",
                    Password = "123"
                };

                // Act
                var result = await service.RegisterUser(createDto);

                // Assert
                result.FirstName.Should().Be(createDto.FirstName);
            }
        }



    }
}