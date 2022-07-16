using Core.Modules.Account.Dtos;
using Core.Modules.Account.Results;

namespace Core.Modules.Account.Services;

public interface IAccountService
{
    Task<RegisterDto> RegisterUser(RegisterDto inputDto, bool sendEmail);
    Task<bool> IsUserExist(string email);
    Task<ActiveAccountResult> ActiveAccount(string activeCode);
    Task<LoginResult> LoginUser(LoginDto loginDto);
}