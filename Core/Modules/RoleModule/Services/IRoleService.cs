using Core.Modules.RoleModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;

namespace Core.Modules.RoleModule.Services;

public interface IRoleService
{
    Task<AdvanceDataTable<RoleDataTableDto>> GetDataTable(AdvanceDataTable<RoleDataTableDto> dataTableRequest);

    Task<OperationResult<RoleUpdateDto>> Create(RoleCreateDto createDto);

    Task<RoleUpdateDto> Get(long id);

    Task<OperationResult<RoleUpdateDto>> Update(RoleUpdateDto updateDto);

    Task<OperationResult<RoleUpdateDto>> Delete(List<RoleDeleteDto> deleteDtos);

    Task<List<PermissionSelectItemDto>> GetPermissionList();

    Task<List<SelectItemDto>> GetSelectItemList();
}