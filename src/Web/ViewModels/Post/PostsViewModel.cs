using Web.Enum;

namespace Web.ViewModels.Post;

public class PostsViewModel
{
    public List<PostItemViewModel> PostItems { get; set; } = null!;
    public PaginationViewModel Pagination { get; set; } = null!;
    public FilterDate? FilterDate { get; set; }
}