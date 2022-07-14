using ApplicationCore.Entities;
using Web.ViewModels.Post;

namespace Web.Interfaces;

public interface ICommentsViewModelService
{
    Task<List<CommentViewModel>> GetSortedPostComments(int postId);
    Task<List<CommentViewModel>> GetCommentReplies(int commentId);
    Task<CommentViewModel> Map(PostComment comment);
}