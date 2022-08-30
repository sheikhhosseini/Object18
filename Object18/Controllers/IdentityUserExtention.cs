using System.Security.Claims;

namespace Object18.Controllers;

public static class IdentityUserExtention
{
    public static long GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal != null)
        {
            var result = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            return Convert.ToInt64(result!.Value);
        }

        return default;
    }
}