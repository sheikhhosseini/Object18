using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Object18.Controllers;

public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string _permissionName;
    public PermissionCheckerAttribute(string permissionName)
    {
        _permissionName = permissionName;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var permissionList = context.HttpContext.User.Claims
            .Where(claim => claim.Type == "permission").ToList();

        var hasPermission = permissionList.Any(claim => claim.Value == _permissionName);

        if (!hasPermission)
        {
            context.Result = new RedirectResult("/AccessDenied");
        }
    }
}