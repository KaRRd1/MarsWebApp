using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces;

public interface IPostService
{
    Task<Post> CreatePostAsync(int userId, string title, string content);
    Task<bool> PostExist(int postId);
    Task<bool> PostHasComment(int postId, int commentId);
    Task RatePostAsync(int userId, int postId, bool isLike);
    Task<PostComment> AddCommentToPostAsync(int userId, string content, int postId, int? parentCommentId);
}