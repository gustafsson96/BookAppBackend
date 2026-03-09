namespace BookAppBackend.Dtos
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public string? BookId { get; set; }
        public string? UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
