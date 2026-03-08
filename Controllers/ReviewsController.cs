using BookAppBackend.Data;
using BookAppBackend.Models;
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
    }
}
