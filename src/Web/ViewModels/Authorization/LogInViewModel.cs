using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Authorization;

public class LogInViewModel
{
    [Required] 
    public string Nickname { get; set; } = null!;
    [Required] 
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}