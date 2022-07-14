using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities;

public class PostRating : BaseEntity, IAggregateRoot
{
    public PostRating(int postId, int userId, bool isLike)
    {
        PostId = postId;
        UserId = userId;
        IsLike = isLike;
    }

    public int PostId { get; private set; }
    public int UserId { get; private set; }
    public bool IsLike { get; private set; }
    public int GetRatingValue()
    {
        return IsLike ? 1 : -1;
    }
}