using BookingSystem.Application.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem.Application.Constants;


namespace BookingSystem.Application.DTOs
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "Типот на услуга е задолжителен")]
        public string ServiceType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Датумот е задолжителен")]
        [DataType(DataType.Date)]
        public DateTime SelectedDate { get; set; }

        [Required(ErrorMessage = "Времето е задолжително")]
        public string SelectedTimeSlot { get; set; } = string.Empty; // Format: "20:00" or "20:30"

        [Required(ErrorMessage = "Времетраењето е задолжително")]
        [Range(1, 10, ErrorMessage = "Времетраењето мора да биде помеѓу 1 и 10 интервали (30 мин секој)")]
        public int DurationInSlots { get; set; } = 1; // Number of 30-minute slots (default 1 = 30 min)

        public string? Notes { get; set; }
    }

    public class AppointmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateAppointmentStatusDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Статусот е задолжителен")]
        public string Status { get; set; } = string.Empty;
    }

    public class TimeSlotDto
    {
        public TimeSpan Time { get; set; }
        public bool IsAvailable { get; set; }
        public string? ReservedBy { get; set; }
    }

    public class AvailabilityDto
    {
        public DateTime Date { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public List<TimeSlotDto> TimeSlots { get; set; } = new();
    }
}