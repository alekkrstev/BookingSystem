namespace BookingSystem.Application.DTOs
{
    public class AvailabilityDto
    {
        public DateTime Date { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public IEnumerable<TimeSlotDto> TimeSlots { get; set; } = new List<TimeSlotDto>();
    }
}