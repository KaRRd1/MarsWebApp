using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class CommentRepository : BaseRepository<PostComment>, ICommentRepository
{
    private readonly MarsContext _dbContext;
    
    public CommentRepository(MarsContext marsContext) : base(marsContext)
    {
        _dbContext = marsContext;
    }

    public IAsyncEnumerable<PostComment> GetSortedPostComments(int postId, int? loggedInUserId)
    {
        return _dbContext.PostComments
            .Where(x => x.PostId == postId && x.ReplyCommentId == null)
            .OrderByDescending(x => x.UserId == loggedInUserId)
            .ThenByDescending(x => x.DateCreated)
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<PostComment> GetSortedReplyComments(int replyCommentId)
    {
        return _dbContext.PostComments
            .Where(x => x.ReplyCommentId == replyCommentId)
            .OrderBy(x => x.DateCreated)
            .AsAsyncEnumerable();
    }
}