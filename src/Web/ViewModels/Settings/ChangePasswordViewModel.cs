using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.Settings;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Field is required")]
    [Remote("CurrentPasswordIsCorrect", "Settings", HttpMethod = "post", ErrorMessage = "Current password is wrong")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "Field is required")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Length must be between 8 and 20 characters")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "Field is required")]
    [Compare("NewPassword", ErrorMessage = "Confirm password do not match")]
    public string ConfirmNewPassword { get; set; } = null!;
}