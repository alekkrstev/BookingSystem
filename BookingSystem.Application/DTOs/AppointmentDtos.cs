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
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty; 
        public string ActivityIcon { get; set; } = string.Empty; 
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
        public int ActivityId { get; set; } 
        public string ActivityName { get; set; } = string.Empty;
        public IEnumerable<TimeSlotDto> TimeSlots { get; set; } = new List<TimeSlotDto>();
    }
}