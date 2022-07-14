using Infrastructure.Identity;
using Web.ViewModels.Post;

namespace Web.ViewModels.Search;

public class SearchIndexViewModel
{
    public List<ProfileViewModel> Profiles { get; set; } = null!;
    public PostsViewModel PostsViewModel { get; set; } = null!;
}