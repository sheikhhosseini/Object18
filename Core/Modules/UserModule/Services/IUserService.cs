using Core.Modules.UserModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;

namespace Core.Modules.UserModule.Services;

public interface IUserService
{
    Task<AdvanceDataTable<UserDataTableDto>> GetDataTable(AdvanceDataTable<UserDataTableDto> dataTableRequest);

    Task<OperationResult<UserUpdateDto>> Create(UserCreateDto createDto);

    Task<UserUpdateDto> Get(long id);

    Task<OperationResult<UserUpdateDto>> Update(UserUpdateDto updateDto);

    Task<OperationResult<UserUpdateDto>> Delete(List<UserDeleteDto> deleteDtos);

    Task<List<SelectItemDto>> SelectItems();
}