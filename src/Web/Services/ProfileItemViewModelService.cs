using ApplicationCore.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services;

public class ProfileItemViewModelService : IProfileItemViewModelService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUriComposer _uriComposer;

    public ProfileItemViewModelService(UserManager<AppUser> userManager, IUriComposer uriComposer)
    {
        _userManager = userManager;
        _uriComposer = uriComposer;
    }

    public async Task<ProfileViewModel?> GetProfileItemByNickname(string nickname)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == nickname);

        if (user != null)
            return Map(user);

        return null;
    }

    public async Task<ProfileViewModel> GetProfileItem(int userId)
    {
        var user = await _userManager.Users.SingleAsync(x => x.Id == userId);

        return Map(user);
    }

    public ProfileViewModel Map(AppUser user)
    {
        return new ProfileViewModel()
        {
            UserId = user.Id,
            Nickname = user.UserName,
            PictureUri = user.PictureUri != null ?_uriComposer.ComposePictureUri(user.PictureUri) : null
        };
    }
}