using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Modules.UserModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Core.Shared.DataTable;

namespace Core.Modules.UserModule.Services;

public class UserService : IUserService
{
    private readonly MainDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDataTableService _dataTable;

    public UserService(
        MainDbContext dbContext,
        IMapper mapper,
        IDataTableService dataTable
    )
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dataTable = dataTable;
    }

    public async Task<AdvanceDataTable<UserDataTableDto>> GetDataTable(
        AdvanceDataTable<UserDataTableDto> dataTableRequest
    )
    {
        return await _dataTable.GetDataTable(_dbContext.GetAsNoTrackingQuery<User>(), dataTableRequest);
    }

    public async Task<OperationResult<UserUpdateDto>> Create(UserCreateDto createDto)
    {
        if (await IsEmailDuplicate(null, createDto.Email))
        {
            return new OperationResult<UserUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "کد ملی تکراری است."
            };
        }

        if (await IsMobileNumberDuplicate(null, createDto.MobileNumber))
        {
            return new OperationResult<UserUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "شماره تلفن تکراری است."
            };
        }

        var newUser = _mapper.Map<User>(createDto);
        string imageName = await FileSaver.CreateImage(createDto.ImageFile, nameof(User));
        newUser.UserImage = imageName;

        await _dbContext.AddEntityAsync(newUser);
        await _dbContext.SaveChangesAsync();

        return new OperationResult<UserUpdateDto>
        {
            Message = "کاربر جدید با موفقیت ایجاد شد.",
            Type = OperationResultType.Single,
            Response = Response.Success
        };
    }

    public async Task<OperationResult<UserUpdateDto>> Update(UserUpdateDto updateDto)
    {
        if (await IsEmailDuplicate(updateDto.Id, updateDto.Email))
        {
            return new OperationResult<UserUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "ایمیل تکراری است."
            };
        }

        if (await IsMobileNumberDuplicate(updateDto.Id, updateDto.MobileNumber))
        {
            return new OperationResult<UserUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "شماره تلفن تکراری است."
            };
        }

        var existingUser = await _dbContext.Users
            .Where(user => user.Id == updateDto.Id)
            .SingleOrDefaultAsync();

        if (existingUser == null) return null;

        _mapper.Map(updateDto, existingUser);

        existingUser.UserImage = await FileSaver.UpdateImage(updateDto.ImageFile, existingUser.UserImage, nameof(User));

        _dbContext.UpdateEntity(existingUser);

        _dbContext.Entry(existingUser).Property(user => user.ConcurrencyStamp).OriginalValue =
            updateDto.ConcurrencyStamp;

        existingUser.ConcurrencyStamp = Guid.NewGuid().ToString();

        await _dbContext.SaveChangesAsync();

        return new OperationResult<UserUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = "کاربر با موفقیت ویرایش شد."
        };
    }

    public async Task<UserUpdateDto> Get(long id)
    {
        return await _dbContext.Users
            .Where(user => user.Id == id)
            .ProjectTo<UserUpdateDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<OperationResult<UserUpdateDto>> Delete(List<UserDeleteDto> deleteDtos)
    {
        if (deleteDtos is null)
            throw new ArgumentNullException(nameof(deleteDtos), "deleteDtos cannot be null.");

        var userIds = deleteDtos.Select(x => x.Id).ToArray();

        var existingUsers = await _dbContext.Users
            .Where(user => userIds.Contains(user.Id))
            .ToListAsync();

        foreach (var existingUser in existingUsers)
        {
            var deleteDto =
                deleteDtos.SingleOrDefault(userDeleteDto => userDeleteDto.Id == existingUser.Id);
            if (deleteDto == null) continue;

            if (string.IsNullOrWhiteSpace(deleteDto.ConcurrencyStamp))
                throw new InvalidOperationException(
                    $"{nameof(deleteDto.ConcurrencyStamp)} for {nameof(User)} with {deleteDto.Id} can not be null or empty."); ;

            _dbContext.Entry(existingUser).Property(user => user.ConcurrencyStamp).OriginalValue =
                deleteDto.ConcurrencyStamp;
        }

        _dbContext.RemoveRange(existingUsers);

        await _dbContext.SaveChangesAsync();

        return new OperationResult<UserUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = $"'{existingUsers.Count}' کاربر با موفقیت حذف شد.",
        };
    }

    public async Task<List<SelectItemDto>> SelectItems()
    {
        return await _dbContext.GetAsNoTrackingQuery<Mission>()
            .Select(mission => new SelectItemDto
            {
                Id = mission.Id.ToString(),
                Text = mission.Title
            }).ToListAsync();
    }

    public async Task<bool> IsEmailDuplicate(long? id, string email)
    {
        return await _dbContext.GetAsNoTrackingQuery<User>()
            .AnyAsync(user => user.Id != id && user.Email == email);
    }

    public async Task<bool> IsMobileNumberDuplicate(long? id, string mobileNumber)
    {
        return await _dbContext.GetAsNoTrackingQuery<User>()
            .AnyAsync(user => user.Id != id && user.MobileNumber == mobileNumber);
    }
}