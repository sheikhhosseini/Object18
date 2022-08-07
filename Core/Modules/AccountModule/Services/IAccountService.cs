using Core.Modules.AccountModule.Dtos;
using Core.Modules.AccountModule.Results;

namespace Core.Modules.AccountModule.Services;

public interface IAccountService
{
    Task<RegisterDto> RegisterUser(RegisterDto inputDto, bool sendEmail);
    Task<bool> IsUserExist(string email);
    Task<ActiveAccountResult> ActiveAccount(string activeCode);
    Task<LoginResult> LoginUser(LoginDto loginDto);
}