using System.ComponentModel.DataAnnotations;

namespace BookAppBackend.Dtos
{
    public class CreateReviewDto
    {
        [Required]
        public string? BookId { get; set; }

        [Required]
        public string? Text { get; set; }

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }
    }
}
