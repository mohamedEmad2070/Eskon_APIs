using System.Security.Claims;

namespace Eskon_APIs.Extensions;

public static class UserExtension
{
    public static string? GetUserId(this ClaimsPrincipal user) => 
        user.FindFirstValue(ClaimTypes.NameIdentifier);
}