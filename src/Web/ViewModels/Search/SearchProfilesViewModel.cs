using Infrastructure.Identity;

namespace Web.ViewModels.Search;

public class SearchProfilesViewModel
{
    public List<ProfileViewModel> Profiles { get; set; } = null!;
    public PaginationViewModel Pagination { get; set; } = null!;
}