namespace BookingSystem.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ActivityId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool HasReview { get; set; } = false; // NEW - track if reviewed

        // Navigation properties
        public User User { get; set; } = null!;
        public Activity Activity { get; set; } = null!;
        public Review? Review { get; set; } // NEW
    }
}