namespace BookingSystem.Application.DTOs
{
    public class TimeSlotDto
    {
        public TimeSpan Time { get; set; }
        public bool IsAvailable { get; set; }
        public string? ReservedBy { get; set; }
    }
}