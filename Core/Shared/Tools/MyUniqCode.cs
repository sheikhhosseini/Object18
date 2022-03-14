namespace Core.Shared.Tools;

public class MyUniqCode
{
    public static string GenerateActiveCode()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }
}