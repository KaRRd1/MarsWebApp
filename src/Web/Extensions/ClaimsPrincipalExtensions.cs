using System.Security.Claims;

namespace Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetLoggedInUserId(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity?.IsAuthenticated == false) 
            return null;

        return int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}