using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using NLog.Web;
using Web.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddNLogWeb();

Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<IdentityContext>();
builder.Services.AddIdentitySettings();

builder.Services.AddCookieSettings();

builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.Logger.LogInformation("App created...");

app.Logger.LogInformation("Seeding Database...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var marsContext = scopedProvider.GetRequiredService<MarsContext>();
        await MarsContextSeed.SeedAsync(marsContext, app.Logger);

        var userManager = scopedProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<AppRole>>();
        var identityContext = scopedProvider.GetRequiredService<IdentityContext>();
        await IdentityContextSeed.SeedAsync(identityContext, userManager, roleManager);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

var marsBaseUrl = builder.Configuration.GetValue(typeof(string), "MarsBaseUrl") as string;
if (!string.IsNullOrEmpty(marsBaseUrl))
{
    app.Use((context, next) =>
    {
        context.Request.PathBase = new PathString(marsBaseUrl);
        return next();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "area",
        pattern: "{area:exists}/{controller}/{action=Index}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Posts}/{action=Index}");
});

app.Logger.LogInformation("Launching");
app.Run();