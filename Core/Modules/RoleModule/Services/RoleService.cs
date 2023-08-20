using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Modules.RoleModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Core.Shared.DataTable;

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
        if (await IsNameDuplicate(null, createDto.Name))
        {
            return new OperationResult<RoleUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "نام تکراری است."
            };
        }

        var newRole = _mapper.Map<Role>(createDto);

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
        if (await IsNameDuplicate(updateDto.Id, updateDto.Name))
        {
            return new OperationResult<RoleUpdateDto>
            {
                Type = OperationResultType.Single,
                Response = Response.Failed,
                Message = "نام تکراری است."
            };
        }

        var existingRole = await _dbContext.Roles
            .Include(role => role.RolePermissions)
            .Where(role => role.Id == updateDto.Id)
            .SingleOrDefaultAsync();

        if (existingRole == null) return null;

        _mapper.Map(updateDto, existingRole);

        var updatedPermissionList = updateDto.PermissionIds.Select(permissionId => new RolePermission
        {
            Id = existingRole.RolePermissions.SingleOrDefault(x => x.PermissionId == permissionId)?.Id ?? 0,
            RoleId = existingRole.Id,
            PermissionId = permissionId
        }).ToList();

        existingRole.RolePermissions = updatedPermissionList;

        _dbContext.UpdateEntity(existingRole);

        _dbContext.Entry(existingRole).Property(role => role.ConcurrencyStamp).OriginalValue =
            updateDto.ConcurrencyStamp;

        existingRole.ConcurrencyStamp = Guid.NewGuid().ToString();

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

        _dbContext.RemoveRange(existingRoles);

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

    public async Task<List<SelectItemDto>> GetSelectItemList()
    {
        return await _dbContext.GetAsNoTrackingQuery<Role>()
            .Select(mission => new SelectItemDto
            {
                Id = mission.Id.ToString(),
                Text = mission.Name
            })
            .ToListAsync();
    }

    private async Task<bool> IsNameDuplicate(long? id, string name)
    {
        return await _dbContext.GetAsNoTrackingQuery<Role>()
            .AnyAsync(role => role.Id != id && role.Name == name);
    }
}