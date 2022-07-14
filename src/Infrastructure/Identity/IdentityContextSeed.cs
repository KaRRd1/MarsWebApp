using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class IdentityContextSeed
{
    public static async Task SeedAsync(IdentityContext identityContext, UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        identityContext.Database.Migrate();

        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new AppRole("User"));
            await roleManager.CreateAsync(new AppRole("Moderator"));
            await roleManager.CreateAsync(new AppRole("Administrator"));
        }

        if (!userManager.Users.Any())
        {
            var user = new AppUser()
            {
                UserName = "Elon",
                Email = "elonTesla@gmail.com"
            };

            await userManager.CreateAsync(user, "qwerty123");
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}