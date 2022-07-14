using ApplicationCore.Interfaces;
using ImageMagick;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;

namespace Web.Controllers;

public class ProfileController : Controller
{
    private readonly IProfileViewModelService _profileViewModelService;
    private readonly IWebHostEnvironment _environment;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUriComposer _uriComposer;

    public ProfileController(IProfileViewModelService profileViewModelService,
        IWebHostEnvironment environment, UserManager<AppUser> userManager, IUriComposer uriComposer)
    {
        _profileViewModelService = profileViewModelService;
        _environment = environment;
        _userManager = userManager;
        _uriComposer = uriComposer;
    }

    [Route("/{controller}/{username}")]
    public async Task<IActionResult> Index(string username, int page = 1)
    {
        var vm = await _profileViewModelService.GetProfileIndex(page, username);

        if (vm == null) 
            return NotFound();

        return View(vm);
    }
    
    
    [HttpPost]
    [Authorize]
    [Route("/{controller}/{action}")]
    public async Task<IActionResult> UpdateAvatar(IFormFile file)
    {
        var user = await _userManager.GetUserAsync(User);

        var uploadedFileFormat = file.FileName.Split('.').Last();
        var fileName = $"{user.UserName}-{DateTime.Now.ToFileTimeUtc()}.{uploadedFileFormat}";
        var path = $"/Images/Profiles/{fileName}";

        if (user.PictureUri != null)
            System.IO.File.Delete(_environment.WebRootPath + _uriComposer.ComposePictureUri(user.PictureUri));

        using (var image = new MagickImage(file.OpenReadStream()))
        {
            image.Resize(256, 0);
            await image.WriteAsync(_environment.WebRootPath + path);
        }

        user.PictureUri = _uriComposer.ComposeBaseUri(path);
        await _userManager.UpdateAsync(user);

        return RedirectToAction("Index", new { username = user.UserName });
    }
}