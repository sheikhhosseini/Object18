using AutoMapper;
using Core.Modules.UserModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
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

    public async Task<AdvanceDataTable<UserDataTableDto>> GetDataTable(
        AdvanceDataTable<UserDataTableDto> data
    )
    {
        var query = _dbContext.GetAsNoTrackingQuery<User>();

        foreach (var filter in data.Filters)
        {
            if (filter.KeyType == "text")
            {
                query = query.ApplyFiltering($"{filter.KeyName} {filter.KeyOperator} {filter.KeyValue.First()}");
            }
            else if (filter.KeyType == "number")
            {
                query = query.ApplyFiltering($"{filter.KeyName} {filter.KeyOperator} {filter.KeyValue.First()}");
            }
            else if (filter.KeyType == "list")
            {

            }
        }

        foreach (var sortOrder in data.SortOrder)
        {
            if (!string.IsNullOrEmpty(sortOrder.KeyName) && !string.IsNullOrEmpty(sortOrder.KeySort))
            {
                query = query.ApplyOrdering($"{sortOrder.KeyName} {sortOrder.KeySort}");
            }
        }

        return await GeneratePages(data, query);
    }

    public async Task<OperationResult<UserUpdateDto>> Create(UserCreateDto createDto)
    {
        var newUser = _mapper.Map<User>(createDto);
        string imageName = await FileSaver.CreateImage(createDto.UserImage, nameof(User));
        newUser.UserImage = imageName;

        await _dbContext.AddEntityAsync(newUser);
        await _dbContext.SaveChangesAsync();

        return new OperationResult<UserUpdateDto>
        {
            Message = "کاربر با موفقیت ایجاد شد",
            Type = OperationResultType.Single,
            Response = Response.Success
        };
    }

    public async Task<UserUpdateDto> Update(UserUpdateDto updateDto)
    {
        var existingUser = await _dbContext.Users
            .Where(u => u.Id == 1)
            .SingleOrDefaultAsync();

        _mapper.Map(updateDto, existingUser);

        _dbContext.UpdateEntity(existingUser);
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
        var existingUser = await _dbContext.Users
            .Where(u => u.Id == id)
            .SingleOrDefaultAsync();

        return _mapper.Map<UserUpdateDto>(existingUser);
    }

    public async Task<OperationResult<UserUpdateDto>> Delete(List<long> deleteDtos)
    {
        var users = await _dbContext.Users
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