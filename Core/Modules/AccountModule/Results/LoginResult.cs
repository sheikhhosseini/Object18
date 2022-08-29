using System.Security.Claims;
using Data.Models;

namespace Core.Modules.AccountModule.Results;

public class LoginResult
{
    public LoginStatus Status { get; set; }
    public User User { get; set; }
    public List<Claim> Claims { get; set; }
}

public enum LoginStatus
{
    Success,
    Failed,
    NotActivated
}