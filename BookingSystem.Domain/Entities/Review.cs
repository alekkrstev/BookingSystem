namespace BookingSystem.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ActivityId { get; set; } // Nullable - за playroom reviews
        public int? AppointmentId { get; set; } // NEW - link to specific appointment
        public string ReviewType { get; set; } = "Activity"; // "Activity" or "Playroom"
        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public Activity? Activity { get; set; }
        public Appointment? Appointment { get; set; } // NEW
    }
}