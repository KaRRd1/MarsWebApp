using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Extensions;
using Web.ViewModels.Authorization;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
public class AuthorizationController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AuthorizationController> _logger;

    public AuthorizationController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        ILogger<AuthorizationController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [TempData] public string? LogInErrorMessage { get; set; }

    public IActionResult Index(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return LocalRedirect(Url.Content("~/"));

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = new AppUser()
            {
                UserName = signUpViewModel.Nickname,
                Email = signUpViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, signUpViewModel.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User[ID: {user.Id}] signed up");
                await _signInManager.SignInAsync(user, isPersistent: true);
                return LocalRedirect(returnUrl);
            }
        }

        return RedirectToAction("Index", new { returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel logInViewModel, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var result = await _signInManager
                .PasswordSignInAsync(logInViewModel.Nickname, logInViewModel.Password, true, false);

            if (result == SignInResult.Success)
            {
                var loggedInUser = await _userManager.FindByNameAsync(logInViewModel.Nickname);
                _logger.LogInformation($"User[ID: {loggedInUser.Id}] logged in");
                return LocalRedirect(returnUrl);
            }
        }

        LogInErrorMessage = "Bad nickname or password";
        return RedirectToAction("Index", new { returnUrl });
    }

    [Authorize]
    public async Task<IActionResult> LogOut(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        _logger.LogInformation($"User[ID: {User.GetLoggedInUserId()}] logged out");
        await _signInManager.SignOutAsync();

        return LocalRedirect(returnUrl);
    }

    [HttpPost]
    public async Task<bool> NicknameIsAvailable(string nickname)
    {
        return !await _userManager.Users.AnyAsync(x => x.UserName == nickname);
    }
}