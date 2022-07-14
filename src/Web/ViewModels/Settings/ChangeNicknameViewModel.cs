using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.Settings;

public class ChangeNicknameViewModel
{
    [Required(ErrorMessage = "Field is required")]
    [StringLength(15, MinimumLength = 4, ErrorMessage = "Length must be between 4 and 15 characters")]
    [Remote("NicknameIsAvailable", "Authorization", areaName: "Account", ErrorMessage = "Nickname is taken",
        HttpMethod = "post")]
    public string Nickname { get; set; } = null!;
}