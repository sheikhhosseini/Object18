using System;
using Core.Shared.Tools;
using Data.Models;
using TestProject.MyRandomGenerator;

namespace TestProject.Factory;

public class UserFactory
{
    public static User Create()
    {
        DateTime currentTime = DateTime.Now;
        return new User
        {
            Email = RandomFactory.Email(),
            FirstName = RandomFactory.FirstName(),
            LastName = RandomFactory.LastName(),
            Password = RandomFactory.Text(5).EncodePasswordMd5(),
            ActiveCode = MyUniqCode.GenerateActiveCode(),
            UserImage = "Default.jpg",
            CreateDate = currentTime,
            LastUpdateDate = currentTime
        };
    }
}