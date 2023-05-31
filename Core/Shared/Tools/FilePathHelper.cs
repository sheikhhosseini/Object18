namespace Core.Shared.Tools;

public static class FilePathHelper
{
    public static string ImagePath(string imageName)
    {
        //return Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Images/{imageName}");
        return $"/Images/{imageName}";

    }
}