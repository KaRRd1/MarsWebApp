using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Enum;
using Web.Extensions;
using Web.Interfaces;
using Web.ViewModels.Post;

namespace Web.Controllers;

public class PostsController : Controller
{
    private readonly IPostsViewModelService _postsViewModelService;
    private readonly ICommentsViewModelService _commentsViewModelService;
    private readonly IPostService _postService;

    public PostsController(IPostsViewModelService postsViewModelService,
        ICommentsViewModelService commentsViewModelService, IPostService postService)
    {
        _postsViewModelService = postsViewModelService;
        _postService = postService;
        _commentsViewModelService = commentsViewModelService;
    }

    public async Task<IActionResult> Index(int page = 1, FilterDate filterDate = FilterDate.ThisWeek)
    {
        var vm = await _postsViewModelService.GetTopPosts(page, filterDate);
        return View(vm);
    }

    [ActionName("New")]
    public async Task<IActionResult> NewPosts(int? page)
    {
        var vm = await _postsViewModelService.GetNewPosts(page ?? 1);
        return View(vm);
    }

    [Route("{controller}/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var vm = await _postsViewModelService.GetPostItemWithComments(id);

        if (vm == null)
            return NotFound();

        return View(vm);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(PostCreateViewModel vm)
    {
        var createdPost = await _postService.CreatePostAsync(User.GetLoggedInUserId()!.Value, vm.Title, vm.Content);
        return RedirectToAction("Details", new { id = createdPost.Id });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Rate(int postId, bool isLike)
    {
        if (!await _postService.PostExist(postId))
            ModelState.AddModelError("Post", "Post with ID:{postId} does not exist");

        if (!ModelState.IsValid) 
            return ValidationProblem();
        
        await _postService.RatePostAsync(User.GetLoggedInUserId()!.Value, postId, isLike);
        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateComment(string content, int postId, int? parentCommentId)
    {
        if (!await _postService.PostExist(postId))
            ModelState.AddModelError("Post", $"Post with ID:{postId} does not exist");
        else if (parentCommentId.HasValue && !await _postService.PostHasComment(postId, parentCommentId.Value))
            ModelState.AddModelError("ParentComment", $"Post does not contain comment with ID:{parentCommentId}");

        if (!ModelState.IsValid) 
            return ValidationProblem();
        
        var comment = await _postService.AddCommentToPostAsync(User.GetLoggedInUserId()!.Value, content, postId,
            parentCommentId);
        var vm = await _commentsViewModelService.Map(comment);

        return PartialView("_comment", vm);
    }

    [HttpPost]
    public async Task<IActionResult> GetCommentReplies(int commentId)
    {
        var comments = await _commentsViewModelService.GetCommentReplies(commentId);
        return PartialView("_CommentsPartial", comments);
    }
}