using AutoMapper;
using Core.Modules.TestModule.Dtos;
using Core.Repository;
using Data.Models;

namespace Core.Modules.TestModule.Services;

public class TestService : ITestService
{
    private readonly IGenericRepository<Role> _testRepository;
    private readonly IMapper _mapper;

    public TestService(IGenericRepository<Role> testRepository, IMapper mapper)
    {
        _testRepository = testRepository;
        _mapper = mapper;
    }

    public async Task AddTest(RoleCreateDto createDto)
    {
        //var role = new Role()
        //{
        //    RoleTitle = createDto.RoleTitle,
        //    RoleDescription = createDto.RoleDescription
        //};
        await _testRepository.AddEntity(_mapper.Map<Role>(createDto));
        await _testRepository.SaveChangesAsync();
    }

    public void Dispose()
    {
        _testRepository?.Dispose();
    }
}