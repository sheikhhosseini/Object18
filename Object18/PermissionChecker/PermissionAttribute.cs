using Core.Modules.AccountModule.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Object18.Controllers;

namespace Object18.PermissionChecker;

public class PermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly string _permissionName;
    private IAccountService _accountService;

    public PermissionAttribute(string permissionName)
    {
        _permissionName = permissionName;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        _accountService = (IAccountService)context.HttpContext
            .RequestServices.GetService(typeof(IAccountService));

        //var permissionList = context.HttpContext.User.Claims
        //    .Where(claim => claim.Type == "permission").ToList();
        //var hasPermission = permissionList.Any(claim => claim.Value == _permissionName);

        bool hasPermission =
            await _accountService!.HasPermission(context.HttpContext.User.GetUserId(), _permissionName);

        if (!hasPermission)
        {
            context.Result = new RedirectResult("/AccessDenied");
        }
    }
}