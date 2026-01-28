namespace BookingSystem.Application.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public int? AppointmentId { get; set; } // NEW
        public string ReviewType { get; set; } = string.Empty; // "Activity" or "Playroom"
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}