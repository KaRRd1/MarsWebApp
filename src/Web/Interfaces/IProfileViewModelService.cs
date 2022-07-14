using Infrastructure.Identity;
using Web.ViewModels;

namespace Web.Interfaces;

public interface IProfileViewModelService
{
    Task<ProfileIndexViewModel?> GetProfileIndex(int page, string nickname);
}