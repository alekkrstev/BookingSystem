using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingSystem.Web.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var appointments = await _appointmentService.GetUserAppointmentsAsync(userId);
            return View(appointments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var appointment = await _appointmentService.CreateAppointmentAsync(userId, model);

            if (appointment == null)
            {
                ModelState.AddModelError("", "Грешка при креирање на терминот. Можеби терминот е веќе зафатен.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Терминот е успешно закажан!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailability(DateTime date, string serviceType)
        {
            if (string.IsNullOrEmpty(serviceType))
            {
                return BadRequest("Service type is required");
            }

            var availability = await _appointmentService.GetAvailabilityAsync(date, serviceType);

            // Format the response for the frontend
            var response = new
            {
                date = availability.Date.ToString("yyyy-MM-dd"),
                serviceType = availability.ServiceType,
                timeSlots = availability.TimeSlots.Select(ts => new
                {
                    time = ts.Time.ToString(@"hh\:mm"),
                    isAvailable = ts.IsAvailable,
                    reservedBy = ts.ReservedBy
                })
            };

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Терминот е успешно избришан!";
            return RedirectToAction("Index");
        }
    }
}