using Infrastructure.Identity;
using Web.ViewModels.Post;

namespace Web.ViewModels;

public class ProfileIndexViewModel
{
    public ProfileViewModel ProfileViewModel { get; set; } = null!;
    public int CommentsCount { get; set; }
    public int PositiveRatings { get; set; }
    public int NegativeRatings { get; set; }
    public int Rating { get; set; }
    public DateTime SignUpDate { get; set; }
    public PostsViewModel PostsViewModel { get; set; } = null!;
}