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
            .Where(x => x.PostId == postId && x.ParentCommentId == null)
            .OrderByDescending(x => x.UserId == loggedInUserId)
            .ThenByDescending(x => x.DateCreated)
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<PostComment> GetSortedReplyComments(int commentId)
    {
        return _dbContext.PostComments
            .Where(x => x.ParentCommentId == commentId)
            .OrderBy(x => x.DateCreated)
            .AsAsyncEnumerable();
    }
}