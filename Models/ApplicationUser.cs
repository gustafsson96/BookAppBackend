using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // Additional field for users real name
    public string DisplayName { get; set; }
}
