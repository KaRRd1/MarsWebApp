using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Post;

public class PostCreateViewModel
{
    [Required(ErrorMessage = "Required")] 
    [MaxLength(200, ErrorMessage = "Max length 200 symbols")] 
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Required")] 
    [MaxLength(5000, ErrorMessage = "Max length 5000 symbols")] 
    public string Content { get; set; } = null!;
}