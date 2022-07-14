namespace Web.ViewModels.Post;

public class PostItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int Raiting { get; set; }
    public bool? IsLiked { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public ProfileViewModel Author { get; set; } = null!;
    public int CommentsCount { get; set; }
    public List<CommentViewModel>? Comments { get; set; }
}