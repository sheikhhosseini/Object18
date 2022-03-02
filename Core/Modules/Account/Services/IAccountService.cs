using Core.Modules.Account.Dtos;

namespace Core.Modules.Account.Services;

public interface IAccountService
{
    Task<RegisterDto> RegisterUser(RegisterDto inputDto);
}