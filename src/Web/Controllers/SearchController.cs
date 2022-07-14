using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;

namespace Web.Controllers;

public class SearchController : Controller
{
    private readonly ISearchViewModelService _searchViewModelService;

    public SearchController(ISearchViewModelService searchViewModelService)
    {
        _searchViewModelService = searchViewModelService;
    }

    public async Task<IActionResult> Index(string query, int page = 1)
    {
        var vm = await _searchViewModelService.GetSearchResult(page, query);
        return View(vm);
    }

    public async Task<IActionResult> Profiles(string query, int page = 1)
    {
        var vm = await _searchViewModelService.GetSearchProfilesResult(page, query);
        return View(vm);
    }
}