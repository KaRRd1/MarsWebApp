using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities;

public class PostComment : BaseEntity, IAggregateRoot
{
    public PostComment(string content, int userId, int postId, int? replyCommentId = null)
    {
        Content = content;
        UserId = userId;
        PostId = postId;
        ReplyCommentId = replyCommentId;
    }
    
    public string Content { get; private set; }
    public DateTime DateCreated { get; private set; } = DateTime.Now;
    public int PostId { get; private set; }
    public int UserId { get; private set; }
    public int? ReplyCommentId { get; private set; }
}