using Data.Models;
using TestProject.MyRandomGenerator;

namespace TestProject.Factory;

public class PermissionFactory
{
    public static Permission Create()
    {
        return new Permission
        {
            Name = RandomFactory.Text(7),
            PermissionAction = new PermissionAction { Name = RandomFactory.Text(5) },
            PermissionArea = new PermissionArea { Name = RandomFactory.Text(5) },
            PermissionModule = new PermissionModule { Name = RandomFactory.Text(5) }
        };
    }
}