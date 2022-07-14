using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.Settings;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
[Authorize]
public class SettingsController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<SettingsController> _logger;

    public SettingsController(UserManager<AppUser> userManager, ILogger<SettingsController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    [TempData] public string? SuccessMessage { get; set; }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var vm = new SettingsIndexViewModel()
        {
            Nickname = user.UserName,
            Email = user.Email
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeNickname(ChangeNicknameViewModel vm)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index");

        var user = await _userManager.GetUserAsync(User);
        var oldNickname = user.UserName;
        user.UserName = vm.Nickname;
        await _userManager.UpdateAsync(user);
        
        _logger.LogInformation($"User[ID: {user.Id}] changed nickname from <{oldNickname}> to <{user.UserName}>");
        SuccessMessage = "Nickname successfully changed";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel vm)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index");

        var user = await _userManager.GetUserAsync(User);
        var oldEmail = user.Email;
        user.Email = vm.NewEmail;
        await _userManager.UpdateAsync(user);

        _logger.LogInformation($"User[ID: {user.Id}] changed email from <{oldEmail}> to <{user.Email}>");
        SuccessMessage = "Email successfully changed";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index");

        var user = await _userManager.GetUserAsync(User);
        var result = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);

        if (result.Succeeded)
        {
            _logger.LogInformation($"User[ID: {user.Id}] changed password");
            SuccessMessage = "Password successfully changed";
        }

        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public async Task<bool> CurrentPasswordIsCorrect(string currentPassword)
    {
        return await _userManager.CheckPasswordAsync(await _userManager.GetUserAsync(User), currentPassword);
    }
}