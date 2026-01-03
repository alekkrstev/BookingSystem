using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BookingSystem.Application.DTOs;

namespace BookingSystem.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDto?> CreateAppointmentAsync(int userId, CreateAppointmentDto dto);
        Task<IEnumerable<AppointmentDto>> GetUserAppointmentsAsync(int userId);
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
        Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
        Task<bool> UpdateAppointmentStatusAsync(int id, string status);
        Task<bool> DeleteAppointmentAsync(int id);
        Task<AvailabilityDto> GetAvailabilityAsync(DateTime date, string serviceType);
        Task<bool> IsTimeSlotAvailableAsync(DateTime date, TimeSpan startTime, TimeSpan endTime, string serviceType);
    }
}