using Microsoft.AspNetCore.Identity;

namespace Web.Configuration;

public static class ConfigureIdentity
{
    public static IServiceCollection AddIdentitySettings(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });
        
        return services;
    }
}