namespace Web.ViewModels.Post;

public class CommentViewModel
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public ProfileViewModel Author { get; set; } = null!;
    public string CommentContent { get; set; } = null!;
    public int CommentRepliesCount { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}