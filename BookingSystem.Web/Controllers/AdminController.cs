using BookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AdminController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> AdminDashboard()
        {
            // Cleanup expired appointments
            await CleanupExpiredAppointmentsAsync();

            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }
        private async Task CleanupExpiredAppointmentsAsync()
        {
            try
            {
                var allAppointments = await _appointmentService.GetAllAppointmentsAsync();

                var expiredAppointments = allAppointments
                    .Where(a => a.EndTime < DateTime.Now &&
                               (a.Status == "Cancelled" || a.Status == "Pending"))
                    .ToList();

                foreach (var appointment in expiredAppointments)
                {
                    await _appointmentService.DeleteAppointmentAsync(appointment.Id);
                }
            }
            catch (Exception)
            {
                // Silent fail
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var result = await _appointmentService.UpdateAppointmentStatusAsync(id, status);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("AdminDashboard");
        }
    }
}