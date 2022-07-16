using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repository;
using Web.Enum;
using Web.Extensions;
using Web.Interfaces;
using Web.ViewModels;
using Web.ViewModels.Post;

namespace Web.Services;

public class PostsViewModelService : IPostsViewModelService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ICommentsViewModelService _commentsViewModelService;
    private readonly IPostRepository _postRepository;
    private readonly IProfileItemViewModelService _profileItemViewModelService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostsViewModelService(IRatingRepository ratingRepository,
        ICommentRepository commentRepository, IPostRepository postRepository, IHttpContextAccessor httpContextAccessor,
        ICommentsViewModelService commentsViewModelService, IProfileItemViewModelService profileItemViewModelService)
    {
        _ratingRepository = ratingRepository;
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _httpContextAccessor = httpContextAccessor;
        _commentsViewModelService = commentsViewModelService;
        _profileItemViewModelService = profileItemViewModelService;
    }

    public async Task<PostsViewModel> GetTopPosts(int page, FilterDate filterDate)
    {
        var startDate = filterDate switch
        {
            FilterDate.Today => DateTime.Now.AddDays(-1),
            FilterDate.ThisWeek => DateTime.Now.AddDays(-7),
            FilterDate.ThisMonth => DateTime.Now.AddMonths(-1),
            _ => DateTime.MaxValue
        };

        var postCount = await _postRepository.GetCountAsync(x => x.DateCreated >= startDate);
        var pagination = new PaginationViewModel(postCount, page, Constants.PostsPerPage);
        var posts = _postRepository
            .GetSortedPostsByRating(x => x.DateCreated >= startDate, pagination.Skip, pagination.ItemsPerPage);

        return new PostsViewModel()
        {
            PostItems = await posts.SelectAwait(async x => await MapPost(x)).ToListAsync(),
            Pagination = pagination,
            FilterDate = filterDate
        };
    }

    public async Task<PostsViewModel> GetNewPosts(int page)
    {
        var startDate = DateTime.Now.AddDays(-1);

        var postCount = await _postRepository.GetCountAsync(x => x.DateCreated >= startDate);
        var pagination = new PaginationViewModel(postCount, page, Constants.PostsPerPage);
        var posts = _postRepository.GetSortedDescAsync(x => x.DateCreated,
            x => x.DateCreated >= startDate, pagination.Skip, pagination.ItemsPerPage);

        return new PostsViewModel()
        {
            PostItems = await posts.SelectAwait(async x => await MapPost(x)).ToListAsync(),
            Pagination = pagination
        };
    }

    public async Task<PostsViewModel> GetUserPosts(int page, int userId)
    {
        var postCount = await _postRepository.GetCountAsync(x => x.UserId == userId);
        var pagination = new PaginationViewModel(postCount, page, Constants.PostsPerPage);
        var posts = _postRepository
            .GetSortedDescAsync(x => x.DateCreated, x => x.UserId == userId, pagination.Skip, pagination.ItemsPerPage);

        return new PostsViewModel()
        {
            PostItems = await posts.SelectAwait(async x => await MapPost(x)).ToListAsync(),
            Pagination = pagination
        };
    }

    public async Task<PostsViewModel> GetPostsByQuery(int page, string query)
    {
        var postCount = await _postRepository.GetCountAsync(x => x.Title.Contains(query));
        var pagination = new PaginationViewModel(postCount, page, Constants.PostsPerPage);
        var posts = _postRepository
            .GetSortedPostsByRating(x => x.Title.Contains(query), pagination.Skip, pagination.ItemsPerPage);

        return new PostsViewModel()
        {
            PostItems = await posts.SelectAwait(async x => await MapPost(x)).ToListAsync(),
            Pagination = pagination
        };
    }

    public async Task<PostItemViewModel?> GetPostItemWithComments(int postId)
    {
        var post = await _postRepository.SingleAsync(x => x.Id == postId);

        if (post == null)
            return null;

        var vm = await MapPost(post);
        vm.Comments = await _commentsViewModelService.GetSortedPostComments(post.Id);

        return vm;
    }

    public async Task<PostItemViewModel> MapPost(Post post)
    {
        var vm = new PostItemViewModel()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Raiting = await _ratingRepository.GetSumPostRating(post.Id),
            DateCreated = post.DateCreated,
            CommentsCount = await _commentRepository.GetCountAsync(x => x.PostId == post.Id),
            Author = await _profileItemViewModelService.GetProfileItem(post.UserId)
        };

        var loggedInUserId = _httpContextAccessor.HttpContext?.User.GetLoggedInUserId();
        if (loggedInUserId != null)
        {
            var ratingItem =
                await _ratingRepository.SingleAsync(x => x.PostId == post.Id && x.UserId == loggedInUserId);
            vm.IsLiked = ratingItem?.IsLike;
        }

        return vm;
    }
}