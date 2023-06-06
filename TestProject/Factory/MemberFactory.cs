using System;
using Data.Models;
using TestProject.MyRandomGenerator;

namespace TestProject.Factory;

public class MemberFactory
{
    public static Member Create()
    {
        DateTime currentTime = DateTime.Now;
        return new Member
        {
            FirstName = RandomFactory.FirstName(),
            LastName = RandomFactory.LastName(),
            Email = RandomFactory.Email(),
            KodMeli = RandomFactory.Text(10),
            MobileNumber = RandomFactory.Text(11),
            DateOfBirth = RandomFactory.Text(10),
            RegisterDate = RandomFactory.Text(10),
            Gender = true,
            CreateDate = currentTime,
            LastUpdateDate = currentTime
        };
    }
}