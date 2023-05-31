namespace Core.Shared.Tools;

public static class FilePathHelper
{
    public static string ImagePath(string imageName)
    {
        if (string.IsNullOrEmpty(imageName)) return $"/Images/no-image.png";

        return $"/Images/{imageName}";
    }
}