using AutoMapper;
using Core.Modules.UserModule.Dtos;
using Core.Shared.Paging;
using Data.Context;
using Data.Models;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserModule.Services;

public class UserService : IUserService
{
    private readonly MainDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserService(MainDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<UserDataTableDto>> GetDataTable(Rule data)
    {
        var query = _dbContext.GetEntitiesAsNoTrackingQuery<User>();
            
        if (data is not null)
        {
            if (data.Filters.Count > 0)
            {
                foreach (var filter in data.Filters)
                {
                    if (!string.IsNullOrEmpty(filter.Value))
                    {
                        query = query.ApplyFiltering($"{filter.Name} =* {filter.Value}");
                    }
                }
            }
            
            query = query.ApplyOrdering($"createDate {data.SortOrder}");
        }


        var result = await query.ToListAsync();

        return _mapper.Map<List<UserDataTableDto>>(result);
    }

    public async Task<UserUpdateDto> Create(UserCreateDto createDto)
    {
        var createUser = _mapper.Map<User>(createDto);
        await _dbContext.AddEntityAsync(createUser);
        await _dbContext.SaveChangesAsync();
        return null;
    }

    public async Task<UserUpdateDto> Update(UserUpdateDto updateDto)
    {
        var existingUser = await _dbContext.GetEntitiesQuery<User>()
            .Where(u => u.Id == 1)
            .SingleOrDefaultAsync();

        _mapper.Map(updateDto, existingUser);

        await _dbContext.UpdateEntityAsync(existingUser);
        await _dbContext.SaveChangesAsync();
        return null;
    }

    public async Task<UserUpdateDto> Get(long id)
    {
        var existingUser = await _dbContext.GetEntitiesQuery<User>()
            .Where(u => u.Id == id)
            .SingleOrDefaultAsync();

        return _mapper.Map<UserUpdateDto>(existingUser);
    }

    public async Task<UserUpdateDto> Delete(List<UserDeleteDto> deleteDtos)
    {
        throw new NotImplementedException();
    }
}