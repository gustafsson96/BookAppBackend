using System.Security.Claims;
using BookAppBackend.Data;
using BookAppBackend.Dtos;
using BookAppBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        // DB context to access reviews table
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/reviews/book/{bookId}
        // Get all reviews for a specific book
        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByBookId(string bookId)
        {
            var reviews = await _context
                .Reviews.Where(r => r.BookId == bookId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(reviews);
        }

        // POST: api/reviews
        // Create a new review for the logged in user
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(CreateReviewDto dto)
        {
            // Get user id from JWT token claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Stop if no user id was found
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            // Prevent same user from creating multiple reviews for the same book
            var existingReview = await _context.Reviews.FirstOrDefaultAsync(r =>
                r.BookId == dto.BookId && r.UserId == userId
            );

            if (existingReview != null)
            {
                return BadRequest("You have already reviewed this book.");
            }

            var review = new Review
            {
                BookId = dto.BookId,
                UserId = userId,
                Text = dto.Text,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }
    }
}
