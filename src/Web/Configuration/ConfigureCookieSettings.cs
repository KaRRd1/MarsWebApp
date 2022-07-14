namespace Web.Configuration;

public static class ConfigureCookieSettings
{
    private const string IdentifierCookieName = "Auth";
    private const int ExpireTimeInDays = 30;

    public static IServiceCollection AddCookieSettings(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Strict;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(ExpireTimeInDays);
            options.SlidingExpiration = true;
            options.LoginPath = "/Account/Authorization";
            options.Cookie.Name = IdentifierCookieName;
        });

        return services;
    }
}