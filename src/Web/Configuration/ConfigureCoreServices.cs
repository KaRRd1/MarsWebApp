using ApplicationCore;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Services;
using Infrastructure.Data.Repositories;

namespace Web.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();

        services.AddSingleton<IUriComposer>(new UriComposer(configuration.Get<MarsSettings>()));
        services.AddScoped<IPostService, PostService>();
        return services;
    }
}