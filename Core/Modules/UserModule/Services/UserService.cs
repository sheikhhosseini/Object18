using AutoMapper;
using Core.Modules.UserModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using Gridify;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

    public async Task<AdvanceDataTable<UserDataTableDto>> GetDataTable(
        AdvanceDataTable<UserDataTableDto> data
    )
    {
        var query = _dbContext.GetEntitiesAsNoTrackingQuery<User>();

        foreach (var filter in data.Filters)
        {
            if (!string.IsNullOrEmpty(filter.Value))
            {
                query = query.ApplyFiltering($"{filter.Name} =* {filter.Value}");
            }
        }
        query = query.ApplyOrdering($"createDate {data.SortOrder}");

        return await GeneratePages(data, query);
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

        _dbContext.UpdateEntityAsync(existingUser);
        await _dbContext.SaveChangesAsync();

        var x = new OperationResult<UserUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = "",
            Record = new UserUpdateDto()
        };
        
        return null;
    }

    public async Task<UserUpdateDto> Get(long id)
    {
        var existingUser = await _dbContext.GetEntitiesQuery<User>()
            .Where(u => u.Id == id)
            .SingleOrDefaultAsync();

        return _mapper.Map<UserUpdateDto>(existingUser);
    }

    public async Task<OperationResult<UserUpdateDto>> Delete(List<long> deleteDtos)
    {
        var users = await _dbContext.GetEntitiesQuery<User>()
            .Where(u => deleteDtos.Contains(u.Id))
            .ToListAsync();

        foreach (var user in users)
        {
            _dbContext.SoftRemoveEntity(user);
        }

        await _dbContext.SaveChangesAsync();

        return new OperationResult<UserUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = $"'{users.Count}' کاربر با موفقیت حذف شد",
        };
    }

    private async Task<AdvanceDataTable<UserDataTableDto>> GeneratePages(
        AdvanceDataTable<UserDataTableDto> data,
        IQueryable<User> query
    )
    {

        var pageCount = (int)Math.Ceiling(query.Count() / (double)data.TakeEntity);
        var pager = PageGenerator.Generate(pageCount, data.PageId, data.TakeEntity);
        var users = await query.Paging(pager).ToListAsync();

        var records = _mapper.Map<List<UserDataTableDto>>(users);

        var rowNumber = pager.SkipEntity;
        foreach (var record in records)
        {
            record.Row = ++rowNumber;
        }

        var result = new AdvanceDataTable<UserDataTableDto>
        {
            Records = records,
            PageId = pager.PageId,
            PageCount = pager.PageCount,
            StartPage = pager.StartPage,
            EndPage = pager.EndPage,
            TakeEntity = pager.TakeEntity,
            SkipEntity = pager.SkipEntity,
            ActivePage = pager.ActivePage,
        };

        return result;
    }
}