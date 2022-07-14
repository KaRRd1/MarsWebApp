using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.Authorization;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Required")]
    [StringLength(15, MinimumLength = 4, ErrorMessage = "Length must be between 4 and 15 characters")]
    [Remote("NicknameIsAvailable", "Authorization", ErrorMessage = "Nickname is taken", HttpMethod = "post")]
    public string Nickname { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Length must be between 8 and 20 characters")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Confirm password do not match")]
    public string ConfirmPassword { get; set; } = null!;
}