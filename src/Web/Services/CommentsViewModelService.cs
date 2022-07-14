using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repository;
using Web.Extensions;
using Web.Interfaces;
using Web.ViewModels.Post;

namespace Web.Services;

public class CommentsViewModelService : ICommentsViewModelService
{
    private readonly IProfileItemViewModelService _profileItemViewModelService;
    private readonly ICommentRepository _commentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentsViewModelService(ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor,
        IProfileItemViewModelService profileItemViewModelService)
    {
        _commentRepository = commentRepository;
        _httpContextAccessor = httpContextAccessor;
        _profileItemViewModelService = profileItemViewModelService;
    }

    public async Task<List<CommentViewModel>> GetSortedPostComments(int postId)
    {
        var loggedInUserId = _httpContextAccessor.HttpContext?.User.GetLoggedInUserId();
        var comments = _commentRepository.GetSortedPostComments(postId, loggedInUserId);

        return await comments.SelectAwait(async x => await Map(x)).ToListAsync();
    }

    public async Task<List<CommentViewModel>> GetCommentReplies(int commentId)
    {
        var comments = _commentRepository.GetSortedReplyComments(commentId);
        
        return await comments.SelectAwait(async x => await Map(x)).ToListAsync();
    }

    public async Task<CommentViewModel> Map(PostComment comment)
    {
        return new CommentViewModel()
        {
            Id = comment.Id,
            PostId = comment.PostId,
            Author = await _profileItemViewModelService.GetProfileItem(comment.UserId),
            CommentContent = comment.Content,
            CommentRepliesCount = await _commentRepository.GetCountAsync(x => x.ReplyCommentId == comment.Id),
            CreatedDate = comment.DateCreated
        };
    }
}