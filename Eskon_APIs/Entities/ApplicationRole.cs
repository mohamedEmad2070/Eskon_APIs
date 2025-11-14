using Microsoft.AspNetCore.Identity;

namespace Eskon_APIs.Entities;

public sealed class ApplicationRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDisabled { get; set; }
}
