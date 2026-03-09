namespace BookAppBackend.Dtos
{
    // DTO used when updating an existing review
    public class UpdateReviewDto
    {
        // Updated review text
        public string Text { get; set; } = string.Empty;

        // Updated rating value
        public int Rating { get; set; }
    }
};
