namespace BookingSystem.Application.DTOs
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameMk { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public int MaxPlayers { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Calculated properties
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int AppointmentCount { get; set; }
    }
}