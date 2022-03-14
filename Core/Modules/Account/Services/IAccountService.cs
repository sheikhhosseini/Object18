using Core.Modules.Account.Dtos;
using Core.Modules.Account.ResultDtos;

namespace Core.Modules.Account.Services;

public interface IAccountService
{
    Task<RegisterDto> RegisterUser(RegisterDto inputDto, bool sendEmail);
    Task<bool> IsUserExist(string email);
    Task<ActiveAccountResultDto> ActiveAccount(string activeCode);
}