using ApplicationCore.Entities;
using Web.Enum;
using Web.ViewModels.Post;

namespace Web.Interfaces;

public interface IPostsViewModelService
{
    Task<PostsViewModel> GetTopPosts(int page, FilterDate filterDate);

    Task<PostsViewModel> GetNewPosts(int page);

    Task<PostsViewModel> GetUserPosts(int page, int userId);

    Task<PostsViewModel> GetPostsByQuery(int page, string query);

    Task<PostItemViewModel?> GetPostItem(int postId);

    Task<PostItemViewModel> MapPost(Post post);
}