using ApplicationCore.Interfaces.Repository;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services;

public class ProfileViewModelService : IProfileViewModelService
{
    private readonly ICommentRepository _commentsRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly IProfileItemViewModelService _profileItemViewModelService;
    private readonly IPostsViewModelService _postsViewModelService;
    private readonly UserManager<AppUser> _userManager;

    public ProfileViewModelService(ICommentRepository commentsRepository, IRatingRepository ratingRepository,
        IProfileItemViewModelService profileItemViewModelService, IPostsViewModelService postsViewModelService, UserManager<AppUser> userManager)
    {
        _commentsRepository = commentsRepository;
        _ratingRepository = ratingRepository;
        _profileItemViewModelService = profileItemViewModelService;
        _postsViewModelService = postsViewModelService;
        _userManager = userManager;
    }

    public async Task<ProfileIndexViewModel?> GetProfileIndex(int page, string nickname)
    {
        var profile = await _profileItemViewModelService.GetProfileItemByNickname(nickname);

        if (profile == null)
            return null;

        var commentsCount = await _commentsRepository.GetCountAsync(x => x.UserId == profile.UserId);
        var positiveRatings = await _ratingRepository.GetCountAsync(x => x.IsLike && x.UserId == profile.UserId);
        var negativeRatings = await _ratingRepository.GetCountAsync(x => !x.IsLike && x.UserId == profile.UserId);
        var signUpDate = (await _userManager.Users.SingleAsync(x => x.Id == profile.UserId)).SignUpDate;

        return new ProfileIndexViewModel()
        {
            ProfileViewModel = profile,
            CommentsCount = commentsCount,
            PositiveRatings = positiveRatings,
            NegativeRatings = negativeRatings,
            SignUpDate = signUpDate,
            Rating = await _ratingRepository.GetSumUserRating(profile.UserId),
            PostsViewModel = await _postsViewModelService.GetUserPosts(page, profile.UserId)
        };
    }
}