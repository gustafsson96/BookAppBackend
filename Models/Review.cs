using System;
using System.ComponentModel.DataAnnotations;

namespace BookAppBackend.Models
{
    // Model for a book review
    public class Review
    {
        // Primary key
        [Key]
        public int Id { get; set; }

        // Book id from Google Books API
        [Required]
        public string? BookId { get; set; }

        // Id for the user who created the review
        [Required]
        public string? UserId { get; set; }

        // Review text
        [Required]
        public string? Text { get; set; }

        // Rating from 0 - 5
        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        // Store when review was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
