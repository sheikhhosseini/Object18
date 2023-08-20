namespace Core.Modules.UserModule.Dtos;

public class UserDeleteDto
{
    public long Id { get; set; }
    public string ConcurrencyStamp { get; set; }
}