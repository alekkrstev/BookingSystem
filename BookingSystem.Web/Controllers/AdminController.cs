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
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
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