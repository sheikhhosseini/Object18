using System.Collections.Generic;
using Data.Models;
using TestProject.MyRandomGenerator;

namespace TestProject.Factory;

public class RoleFactory
{
    public static Role Create()
    {
        return new Role
        {
            Name = RandomFactory.Text(7),
            Description = RandomFactory.Text(10),
            RolePermissions = new List<RolePermission> { new() { Permission = PermissionFactory.Create() } }
        };
    }
}