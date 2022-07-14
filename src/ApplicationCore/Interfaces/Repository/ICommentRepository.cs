using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.Repository;

public interface ICommentRepository : IBaseRepository<PostComment>
{
    IAsyncEnumerable<PostComment> GetSortedPostComments(int postId, int? loggedInUserId);
    IAsyncEnumerable<PostComment> GetSortedReplyComments(int replyCommentId);
}