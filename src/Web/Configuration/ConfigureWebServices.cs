using Web.Interfaces;
using Web.Services;

namespace Web.Configuration;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<IPostsViewModelService, PostsViewModelService>();
        services.AddScoped<ICommentsViewModelService, CommentsViewModelService>();
        services.AddScoped<IProfileItemViewModelService, ProfileItemViewModelService>();
        services.AddScoped<IProfileViewModelService, ProfileViewModelService>();
        services.AddScoped<ISearchViewModelService, SearchViewModelService>();
        
        return services;
    }
}