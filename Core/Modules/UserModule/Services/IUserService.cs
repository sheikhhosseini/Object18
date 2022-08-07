using Core.Modules.UserModule.Dtos;

namespace Core.Modules.UserModule.Services;

public interface IUserService
{
    Task<List<UserDataTableDto>> GetDataTable(List<Filter> data);

    Task<UserUpdateDto> Create(UserCreateDto createDto);
    
    Task<UserUpdateDto> Update(UserUpdateDto updateDto);

    Task<UserUpdateDto> Get(long id);

    Task<UserUpdateDto> Delete(List<UserDeleteDto> deleteDtos);
}