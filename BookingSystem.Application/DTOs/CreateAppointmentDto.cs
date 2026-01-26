using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Application.DTOs
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "Активноста е задолжителна")]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Датумот е задолжителен")]
        public DateTime SelectedDate { get; set; }

        [Required(ErrorMessage = "Времето е задолжително")]
        public string SelectedTimeSlot { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Времетраењето мора да биде од 1 до 6 слота")]
        public int DurationInSlots { get; set; } = 1;

        public string? Notes { get; set; }
    }
}