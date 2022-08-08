using Core.Modules.UserModule.Dtos;
using Core.Shared.Paging;

namespace Core.Modules.UserModule.Services;

public interface IUserService
{
    Task<AdvanceDataTable<UserDataTableDto>> GetDataTable(AdvanceDataTable<UserDataTableDto> data);

    Task<UserUpdateDto> Create(UserCreateDto createDto);
    
    Task<UserUpdateDto> Update(UserUpdateDto updateDto);

    Task<UserUpdateDto> Get(long id);

    Task<UserUpdateDto> Delete(List<UserDeleteDto> deleteDtos);
}