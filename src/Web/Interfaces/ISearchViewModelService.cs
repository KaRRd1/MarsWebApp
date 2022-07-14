using Web.ViewModels.Search;

namespace Web.Interfaces;

public interface ISearchViewModelService
{
    Task<SearchIndexViewModel> GetSearchResult(int page, string query);
    Task<SearchProfilesViewModel> GetSearchProfilesResult(int page, string query);
}