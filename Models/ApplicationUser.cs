using Microsoft.AspNetCore.Identity;

namespace BookAppBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional field for users real name
        public string? DisplayName { get; set; }
    }
}
