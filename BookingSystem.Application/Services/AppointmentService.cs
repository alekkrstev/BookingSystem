using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;

namespace BookingSystem.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IActivityRepository _activityRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IActivityRepository activityRepository)
        {
            _appointmentRepository = appointmentRepository;
            _activityRepository = activityRepository;
        }

        public async Task<AppointmentDto?> CreateAppointmentAsync(int userId, CreateAppointmentDto dto)
        {
            // Validate activity exists and is active
            var activity = await _activityRepository.GetByIdAsync(dto.ActivityId);
            if (activity == null || !activity.IsActive)
            {
                return null;
            }

            // Parse the selected time slot (format: "HH:mm")
            if (!TimeSpan.TryParse(dto.SelectedTimeSlot, out var startTime))
            {
                return null;
            }

            // Validate that start time is on 30-minute boundary
            if (startTime.Minutes != 0 && startTime.Minutes != 30)
            {
                return null; // Only allow :00 or :30
            }

            var startDateTime = dto.SelectedDate.Date + startTime;
            var endDateTime = startDateTime.AddMinutes(30 * dto.DurationInSlots);

            // Validate future date
            if (startDateTime <= DateTime.Now)
            {
                return null;
            }

            // Check if all time slots are available
            var isAvailable = await IsTimeSlotAvailableAsync(dto.SelectedDate, startTime, endDateTime.TimeOfDay, dto.ActivityId);
            if (!isAvailable)
            {
                return null;
            }

            var appointment = new Appointment
            {
                UserId = userId,
                ActivityId = dto.ActivityId,
                StartTime = startDateTime,
                EndTime = endDateTime,
                Notes = dto.Notes,
                Status = "Pending"
            };

            var created = await _appointmentRepository.AddAsync(appointment);

            return new AppointmentDto
            {
                Id = created.Id,
                UserId = created.UserId,
                UserName = created.User?.UserName ?? "",
                ActivityId = created.ActivityId,
                ActivityName = activity.NameMk,
                ActivityIcon = activity.Icon,
                StartTime = created.StartTime,
                EndTime = created.EndTime,
                Status = created.Status,
                Notes = created.Notes,
                CreatedAt = created.CreatedAt
            };
        }

        public async Task<IEnumerable<AppointmentDto>> GetUserAppointmentsAsync(int userId)
        {
            var appointments = await _appointmentRepository.GetByUserIdAsync(userId);

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.User?.UserName ?? "",
                ActivityId = a.ActivityId,
                ActivityName = a.Activity?.NameMk ?? "",
                ActivityIcon = a.Activity?.Icon ?? "",
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status,
                Notes = a.Notes,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.User?.UserName ?? "",
                ActivityId = a.ActivityId,
                ActivityName = a.Activity?.NameMk ?? "",
                ActivityIcon = a.Activity?.Icon ?? "",
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status,
                Notes = a.Notes,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return null;

            return new AppointmentDto
            {
                Id = appointment.Id,
                UserId = appointment.UserId,
                UserName = appointment.User?.UserName ?? "",
                ActivityId = appointment.ActivityId,
                ActivityName = appointment.Activity?.NameMk ?? "",
                ActivityIcon = appointment.Activity?.Icon ?? "",
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                Notes = appointment.Notes,
                CreatedAt = appointment.CreatedAt
            };
        }

        public async Task<bool> UpdateAppointmentStatusAsync(int id, string status)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return false;

            appointment.Status = status;
            await _appointmentRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return false;

            await _appointmentRepository.DeleteAsync(id);
            return true;
        }

        public async Task<AvailabilityDto> GetAvailabilityAsync(DateTime date, int activityId)
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            var activity = await _activityRepository.GetByIdAsync(activityId);

            // Filter appointments for the specific date and activity
            var dayAppointments = appointments
                .Where(a => a.StartTime.Date == date.Date &&
                           a.ActivityId == activityId &&
                           a.Status != "Cancelled")
                .ToList();

            var timeSlots = new List<TimeSlotDto>();

            // Generate time slots from 10:00 to 23:30 in 30-minute intervals
            for (int hour = 10; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30)
                {
                    var slotTime = new TimeSpan(hour, minute, 0);

                    // Stop at 23:30
                    if (slotTime >= new TimeSpan(24, 0, 0))
                        break;

                    var slotDateTime = date.Date + slotTime;
                    var slotEndDateTime = slotDateTime.AddMinutes(30);

                    // Check if this slot is occupied
                    var isOccupied = dayAppointments.Any(a =>
                        slotDateTime < a.EndTime && slotEndDateTime > a.StartTime);

                    var reservedBy = isOccupied
                        ? dayAppointments.FirstOrDefault(a => slotDateTime < a.EndTime && slotEndDateTime > a.StartTime)?.User?.UserName
                        : null;

                    timeSlots.Add(new TimeSlotDto
                    {
                        Time = slotTime,
                        IsAvailable = !isOccupied,
                        ReservedBy = reservedBy
                    });
                }
            }

            return new AvailabilityDto
            {
                Date = date,
                ActivityId = activityId,
                ActivityName = activity?.NameMk ?? "",
                TimeSlots = timeSlots
            };
        }

        public async Task<bool> IsTimeSlotAvailableAsync(DateTime date, TimeSpan startTime, TimeSpan endTime, int activityId)
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            var startDateTime = date.Date + startTime;
            var endDateTime = date.Date + endTime;

            // Check if there's any overlap with existing appointments
            var hasOverlap = appointments.Any(a =>
                a.ActivityId == activityId &&
                a.Status != "Cancelled" &&
                a.StartTime < endDateTime &&
                a.EndTime > startDateTime);

            return !hasOverlap;
        }
    }
}