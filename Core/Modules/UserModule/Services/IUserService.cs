using Core.Modules.UserModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;

namespace Core.Modules.UserModule.Services;

public interface IUserService
{
    Task<AdvanceDataTable<UserDataTableDto>> GetDataTable(AdvanceDataTable<UserDataTableDto> data);

    Task<OperationResult<UserUpdateDto>> Create(UserCreateDto createDto);
    
    Task<UserUpdateDto> Update(UserUpdateDto updateDto);

    Task<UserUpdateDto> Get(long id);

    Task<OperationResult<UserUpdateDto>> Delete(List<long> deleteDtos);
}