using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace ApplicationCore.Services;

public class PostService : IPostService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostService> _logger;

    public PostService(IRatingRepository ratingRepository, ICommentRepository commentRepository,
        IPostRepository postRepository, ILogger<PostService> logger)
    {
        _ratingRepository = ratingRepository;
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _logger = logger;
    }

    public async Task<Post> CreatePostAsync(int userId, string title, string content)
    {
        var createdPost = await _postRepository.AddAsync(new Post(title, content, userId));
        _logger.LogInformation($"User[ID:{userId}] created post[ID: {createdPost.Id}]");

        return createdPost;
    }

    public async Task<bool> PostExist(int postId)
    {
        return await _postRepository.AnyAsync(x => x.Id == postId);
    }

    public async Task<bool> PostHasComment(int postId, int commentId)
    {
        return await _commentRepository.AnyAsync(x => x.PostId == postId && x.Id == commentId);
    }

    public async Task RatePostAsync(int userId, int postId, bool isLike)
    {
        var currentRate = await _ratingRepository.SingleAsync(x => x.UserId == userId && x.PostId == postId);

        if (currentRate == null)
            await _ratingRepository.AddAsync(new PostRating(postId, userId, isLike));
        else
            await _ratingRepository.DeleteAsync(currentRate);
    }

    public async Task<PostComment> AddCommentToPostAsync(int userId, string content, int postId, int? parentCommentId)
    {
        var newComment = await _commentRepository.AddAsync(new PostComment(content, userId, postId, parentCommentId));
        
        if (parentCommentId.HasValue)
            _logger.LogInformation($"User[ID: {userId}] replied to comment[ID: {parentCommentId}] in a post[ID: {postId}]");
        _logger.LogInformation($"User[ID: {userId}] added comment to post[ID: {postId}]");
        
        return newComment;
    }
}