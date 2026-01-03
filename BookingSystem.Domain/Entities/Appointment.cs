using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ServiceType { get; set; } = string.Empty; // Changed from ServiceName
        public DateTime StartTime { get; set; } // Changed from AppointmentDate
        public DateTime EndTime { get; set; } // NEW - to track duration
        public string Status { get; set; } = "Pending"; // "Pending", "Confirmed", "Cancelled"
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public User User { get; set; } = null!;
    }
}