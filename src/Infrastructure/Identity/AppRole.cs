using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppRole : IdentityRole<int>
{
    public AppRole() {}
    public AppRole(string roleName) : base(roleName)
    {
    }
}