namespace Core.Shared.Tools;

public class MyUniqCode
{
    public static string ActiveCode()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }
}