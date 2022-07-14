using Infrastructure.Identity;
using Web.ViewModels;

namespace Web.Interfaces;

public interface IProfileItemViewModelService
{
    Task<ProfileViewModel?> GetProfileItemByNickname(string nickname);
    Task<ProfileViewModel> GetProfileItem(int userId);
    ProfileViewModel Map(AppUser user);
}