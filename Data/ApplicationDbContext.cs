using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Connection between application and database
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Represents table with Review-objects in the database
    public DbSet<Review> Reviews { get; set; }

    // Constructor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}
