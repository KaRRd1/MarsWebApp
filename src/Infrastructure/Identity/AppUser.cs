using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppUser : IdentityUser<int>
{
    public string? PictureUri { get; set; }
    public DateTime SignUpDate { get; set; } = DateTime.Now;
}