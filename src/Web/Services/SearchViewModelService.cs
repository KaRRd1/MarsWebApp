using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Interfaces;
using Web.ViewModels;
using Web.ViewModels.Search;

namespace Web.Services;

public class SearchViewModelService : ISearchViewModelService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPostsViewModelService _postsViewModelService;
    private readonly IProfileItemViewModelService _profileItemViewModelService;


    public SearchViewModelService(UserManager<AppUser> userManager, IPostsViewModelService postsViewModelService,
        IProfileItemViewModelService profileItemViewModelService)
    {
        _userManager = userManager;
        _postsViewModelService = postsViewModelService;
        _profileItemViewModelService = profileItemViewModelService;
    }

    public async Task<SearchIndexViewModel> GetSearchResult(int page, string query)
    {
        var users = _userManager.Users
            .Where(x => x.UserName.Contains(query))
            .OrderBy(x => x.UserName)
            .Take(15).AsAsyncEnumerable();
        
        return new SearchIndexViewModel()
        {
            Profiles = await users.Select(x => _profileItemViewModelService.Map(x)).ToListAsync(),
            PostsViewModel = await _postsViewModelService.GetPostsByQuery(page, query)
        };
    }

    public async Task<SearchProfilesViewModel> GetSearchProfilesResult(int page, string query)
    {
        var pagination =
            new PaginationViewModel(await _userManager.Users.CountAsync(x => x.UserName.Contains(query)), page, 15);

        var users = _userManager.Users
            .Where(x => x.UserName.Contains(query))
            .OrderBy(x => x.UserName)
            .Skip((page - 1) * pagination.ItemsPerPage)
            .Take(pagination.ItemsPerPage)
            .AsAsyncEnumerable();

        return new SearchProfilesViewModel()
        {
            Profiles = await users.Select(x => _profileItemViewModelService.Map(x)).ToListAsync(),
            Pagination = pagination
        };
    }
}