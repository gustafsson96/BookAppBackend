using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookAppBackend.Controllers
{
    // AuthController
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register new user with email and password
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // Null check
            if (
                string.IsNullOrEmpty(model.Email)
                || string.IsNullOrEmpty(model.Password)
                || string.IsNullOrEmpty(model.DisplayName)
            )
                return BadRequest("Email, password and display name are required.");

            // Create new Identity user object
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                DisplayName = model.DisplayName,
            };

            // Create user with password
            var result = await _userManager.CreateAsync(user, model.Password);

            // Return error if unsuccessful
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Return name if user has been created successfully
            return Ok(new { user.DisplayName });
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // Null check
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Email and password are required.");

            // Get user based on email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized();

            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized();

            // Get secret key to generate token
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(jwtKey))
                throw new Exception("JWT_SECRET is not set in environment variables.");

            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

            // Define token content
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName!) }),
                // Token expires after an hour
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            // Create token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            // Return token and name of user to display in frontend
            return Ok(new { token, displayName = user.DisplayName });
        }
    }
}
