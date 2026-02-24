using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Identity
builder
    .Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

// Identifies and validates user (checks for JWT and sets HttpContext.User)
app.UseAuthentication();

// Handles what an authenticated user is allowed to access
app.UseAuthorization();

// Connect controllers to routing
app.MapControllers();

app.Run();
