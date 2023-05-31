using Microsoft.AspNetCore.Http;
using System.IO;

namespace Core.Shared.Tools;

public class FileSaver
{
    //public static async Task<string> CreateImage(IFormFile file)
    //{
    //    string imageName;
    //    if (file != null)
    //    {
    //        imageName = MyUniqCode.GenerateActiveCode() + Path.GetExtension(file.FileName);

    //        string imagePath = Path.Combine(Directory.GetCurrentDirectory(),
    //            "wwwroot/Images/UserAvatar", imageName);

    //        await using var stream = new FileStream(imagePath, FileMode.Create);
    //        await file.CopyToAsync(stream);
    //    }
    //    else
    //    {
    //        imageName = "Default.jpg";
    //    }
    //    return imageName;
    //}

    //public static async Task<string> UpdateImage(IFormFile file, string oldImage)
    //{
    //    string imageName;
    //    if (file != null)
    //    {
    //        string imagePath;
    //        if (oldImage != "Default.jpg")
    //        {
    //            imagePath = Path.Combine(Directory.GetCurrentDirectory(),
    //                "wwwroot/Images/UserAvatar", oldImage);
    //            if (File.Exists(imagePath))
    //            {
    //                File.Delete(imagePath);
    //            }
    //        }

    //        imageName = MyUniqCode.GenerateActiveCode() + Path.GetExtension(file.FileName);

    //        imagePath = Path.Combine(Directory.GetCurrentDirectory(),
    //            "wwwroot/Images/UserAvatar", imageName);

    //        await using var stream = new FileStream(imagePath, FileMode.Create);
    //        await file.CopyToAsync(stream);
    //    }
    //    else
    //    {
    //        imageName = oldImage;
    //    }
    //    return imageName;
    //}



    public static async Task<string> CreateImage(IFormFile file, string folderName)
    {
        string imageName = null;

        if (file != null)
        {
            imageName = MyUniqCode.GenerateActiveCode() + Path.GetExtension(file.FileName);

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Images/{folderName}");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string imagePath = Path.Combine(folderPath, imageName);

            await using var stream = new FileStream(imagePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        return imageName;
    }

    public static async Task<string> UpdateImage(IFormFile file, string oldImage, string folderName)
    {
        string imageName;
        if (file != null)
        {
            if (!string.IsNullOrEmpty(oldImage))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                    $"wwwroot/Images/{folderName}", oldImage);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            imageName = await CreateImage(file, folderName);
        }
        else
        {
            imageName = oldImage;
        }
        return imageName;
    }
}