using Data.Models;

namespace Core.Modules.Account.Results;

public class LoginResult
{
    public LoginStatus Status { get; set; }
    public User User { get; set; }
}

public enum LoginStatus
{
    Success,
    Failed,
    NotActivated
}