namespace BookingSystem.Application.DTOs
{
    public class QuoteDto
    {
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string GamingContext { get; set; } = string.Empty; // TRANSFORMATION
        public string Icon { get; set; } = string.Empty; // TRANSFORMATION
    }
}