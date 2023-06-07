using System.ComponentModel;
using System.Reflection;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Object18.Areas.Admin.Controllers;

namespace Object18.PermissionChecker;

public class DynamicPermission
{
    public static void GenerateDynamicPermissions(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var typesList = Assembly
            .GetExecutingAssembly()
            .ExportedTypes
            .Where(t => t.IsClass && typeof(IControllerPermissions).IsAssignableFrom(t))
            .ToList();

        var systemPermissions = new List<PermissionTempModel>();

        foreach (var type in typesList)
        {
            foreach (var fieldInfo in type.GetFields())
            {
                var permission = new PermissionTempModel
                {
                    Name = fieldInfo.GetRawConstantValue()?.ToString(),

                    Module = ((DisplayNameAttribute)type.GetCustomAttributes(
                        typeof(DisplayNameAttribute), true).FirstOrDefault())?.DisplayName,

                    Action = ((DescriptionAttribute)fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true)
                        .FirstOrDefault())?.Description,
                };

                permission.Area = permission.Name?.Substring(0, permission.Name.IndexOf('.'));

                systemPermissions.Add(permission);
            }
        }

        var services = serviceScope.ServiceProvider;
        var dbContext = services.GetService<MainDbContext>();

        var dbPermissions = dbContext.Set<Permission>()
            .Include(p => p.PermissionArea)
            .Include(p => p.PermissionModule)
            .Include(p => p.PermissionAction)
            .ToList();

        var dbAreas = dbPermissions.GroupBy(p => p.PermissionAreaId)
            .SelectMany(g => g.Select(p => p.PermissionArea))
            .ToList();

        var dbModules = dbPermissions.GroupBy(p => p.PermissionModuleId)
            .SelectMany(g => g.Select(p => p.PermissionModule))
            .ToList();

        var dbActions = dbPermissions.GroupBy(p => p.PermissionActionId)
            .SelectMany(g => g.Select(p => p.PermissionAction))
            .ToList();

        var newPermissions = new List<Permission>();

        foreach (var sysPermission in systemPermissions)
        {
            var dbPermission = dbPermissions
                .FirstOrDefault(p => p.Name == sysPermission.Name);

            if (dbPermission is not null)
                continue;

            var dbArea = dbAreas.FirstOrDefault(a => a.Name == sysPermission.Area);

            if (dbArea is null)
            {
                dbArea = new PermissionArea
                {
                    Name = sysPermission.Area
                };

                dbAreas.Add(dbArea);

                dbContext.Add(dbArea);
            }

            var dbModule = dbModules.FirstOrDefault(m => m.Name == sysPermission.Module);

            if (dbModule is null)
            {
                dbModule = new PermissionModule
                {
                    Name = sysPermission.Module
                };

                dbModules.Add(dbModule);

                dbContext.Add(dbModule);
            }

            var dbAction = dbActions.FirstOrDefault(m => m.Name == sysPermission.Action);

            if (dbAction is null)
            {
                dbAction = new PermissionAction
                {
                    Name = sysPermission.Action
                };

                dbActions.Add(dbAction);

                dbContext.Add(dbAction);
            }

            var newPermission = new Permission
            {
                Name = sysPermission.Name,
                PermissionArea = dbArea,
                PermissionModule = dbModule,
                PermissionAction = dbAction
            };

            newPermissions.Add(newPermission);
        }

        dbContext.AddRange(newPermissions);

        dbContext.SaveChanges();
    }
}

public class PermissionTempModel
{
    public string Name { get; set; }

    public string Area { get; set; }

    public string Module { get; set; }

    public string Action { get; set; }
}