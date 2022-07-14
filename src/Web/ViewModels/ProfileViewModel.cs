namespace Web.ViewModels;

public class ProfileViewModel
{
    public int UserId { get; set; }
    public string Nickname { get; set; } = null!;
    public string? PictureUri { get; set; }
}