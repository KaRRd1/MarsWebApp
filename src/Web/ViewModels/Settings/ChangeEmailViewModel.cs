using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Settings;

public class ChangeEmailViewModel
{
    [Required(ErrorMessage = "Field is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string NewEmail { get; set; } = null!;
}