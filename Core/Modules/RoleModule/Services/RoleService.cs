using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Modules.RoleModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Core.Shared.DataTable;
using System.Threading;
using Data.Migrations;

namespace Core.Modules.RoleModule.Services;

public class RoleService : IRoleService
{
    private readonly MainDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDataTableService _dataTable;

    public RoleService(
        MainDbContext dbContext,
        IMapper mapper,
        IDataTableService dataTable
    )
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dataTable = dataTable;
    }

    public async Task<AdvanceDataTable<RoleDataTableDto>> GetDataTable(
        AdvanceDataTable<RoleDataTableDto> dataTableRequest
    )
    {
        return await _dataTable.GetDataTable(_dbContext.GetAsNoTrackingQuery<Role>(), dataTableRequest);
    }

    public async Task<OperationResult<RoleUpdateDto>> Create(RoleCreateDto createDto)
    {
        //if (await IsKodMeliDuplicate(null, createDto.KodMeli))
        //{
        //    return new OperationResult<RoleUpdateDto>
        //    {
        //        Type = OperationResultType.Single,
        //        Response = Response.Failed,
        //        Message = "کد ملی تکراری است."
        //    };
        //}

        //if (await IsMobileNumberDuplicate(null, createDto.MobileNumber))
        //{
        //    return new OperationResult<RoleUpdateDto>
        //    {
        //        Type = OperationResultType.Single,
        //        Response = Response.Failed,
        //        Message = "شماره تلفن تکراری است."
        //    };
        //}

        var newRole = _mapper.Map<Role>(createDto);
        //string imageName = await FileSaver.CreateImage(createDto.ImageFile, nameof(Role));
        //newRole.Image = imageName;

        await _dbContext.AddEntityAsync(newRole);
        await _dbContext.SaveChangesAsync();

        return new OperationResult<RoleUpdateDto>
        {
            Message = "نقش جدید با موفقیت ایجاد شد.",
            Type = OperationResultType.Single,
            Response = Response.Success
        };
    }

    public async Task<OperationResult<RoleUpdateDto>> Update(RoleUpdateDto updateDto)
    {
        //if (await IsKodMeliDuplicate(updateDto.Id, updateDto.KodMeli))
        //{
        //    return new OperationResult<RoleUpdateDto>
        //    {
        //        Type = OperationResultType.Single,
        //        Response = Response.Failed,
        //        Message = "کد ملی تکراری است."
        //    };
        //}

        //if (await IsMobileNumberDuplicate(updateDto.Id, updateDto.MobileNumber))
        //{
        //    return new OperationResult<RoleUpdateDto>
        //    {
        //        Type = OperationResultType.Single,
        //        Response = Response.Failed,
        //        Message = "شماره تلفن تکراری است."
        //    };
        //}

        var existingRole = await _dbContext.Roles
            .Where(role => role.Id == updateDto.Id)
            .SingleOrDefaultAsync();

        if (existingRole == null) return null;

        _mapper.Map(updateDto, existingRole);

        //existingRole.Image = await FileSaver.UpdateImage(updateDto.ImageFile, existingRole.Image, nameof(Role));

        _dbContext.UpdateEntity(existingRole);

        //_dbContext.Entry(existingRole).Property(role => role.ConcurrencyStamp).OriginalValue =
        //    updateDto.ConcurrencyStamp;

        //existingRole.ConcurrencyStamp = Guid.NewGuid().ToString();

        await _dbContext.SaveChangesAsync();

        return new OperationResult<RoleUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = "نقش با موفقیت ویرایش شد."
        };
    }

    public async Task<RoleUpdateDto> Get(long id)
    {
        return await _dbContext.Roles
            .Where(role => role.Id == id)
            .ProjectTo<RoleUpdateDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<OperationResult<RoleUpdateDto>> Delete(List<RoleDeleteDto> deleteDtos)
    {
        if (deleteDtos is null)
            throw new ArgumentNullException(nameof(deleteDtos), "deleteDtos cannot be null.");

        var roleIds = deleteDtos.Select(x => x.Id).ToArray();

        var existingRoles = await _dbContext.Roles
            .Where(role => roleIds.Contains(role.Id))
            .ToListAsync();

        foreach (var existingRole in existingRoles)
        {
            var deleteDto =
                deleteDtos.SingleOrDefault(roleDeleteDto => roleDeleteDto.Id == existingRole.Id);
            if (deleteDto == null) continue;

            if (string.IsNullOrWhiteSpace(deleteDto.ConcurrencyStamp))
                throw new InvalidOperationException(
                    $"{nameof(deleteDto.ConcurrencyStamp)} for {nameof(Role)} with {deleteDto.Id} can not be null or empty."); ;

            _dbContext.Entry(existingRole).Property(role => role.ConcurrencyStamp).OriginalValue =
                deleteDto.ConcurrencyStamp;
        }

        _dbContext.SoftRemoveEntities(existingRoles);

        await _dbContext.SaveChangesAsync();

        return new OperationResult<RoleUpdateDto>
        {
            Type = OperationResultType.Single,
            Response = Response.Success,
            Message = $"'{existingRoles.Count}' نقش با موفقیت حذف شد.",
        };
    }

    public async Task<List<PermissionSelectItemDto>> GetPermissionList()
    {
        return await _dbContext.GetAsNoTrackingQuery<Permission>()
            .ProjectTo<PermissionSelectItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    //public async Task<bool> IsKodMeliDuplicate(long? id, string kodMeli)
    //{
    //    return await _dbContext.GetAsNoTrackingQuery<Role>()
    //        .AnyAsync(role => role.Id != id && role.KodMeli == kodMeli);
    //}

    //public async Task<bool> IsMobileNumberDuplicate(long? id, string mobileNumber)
    //{
    //    return await _dbContext.GetAsNoTrackingQuery<Role>()
    //        .AnyAsync(role => role.Id != id && role.MobileNumber == mobileNumber);
    //}
}