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
        private readonly IActivityService _activityService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IActivityService activityService)
        {
            _appointmentService = appointmentService;
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Cleanup expired appointments
            await CleanupExpiredAppointmentsAsync();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var appointments = await _appointmentService.GetUserAppointmentsAsync(userId);
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? activityId)
        {
            var model = new CreateAppointmentDto
            {
                SelectedDate = DateTime.Today
            };

            // Preselect activity if provided
            if (activityId.HasValue)
            {
                model.ActivityId = activityId.Value;
            }

            // Load activities for dropdown
            ViewBag.Activities = await _activityService.GetActiveActivitiesAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Activities = await _activityService.GetActiveActivitiesAsync();
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var appointment = await _appointmentService.CreateAppointmentAsync(userId, model);

            if (appointment == null)
            {
                ModelState.AddModelError("", "Грешка при креирање на терминот. Можеби терминот е веќе зафатен.");
                ViewBag.Activities = await _activityService.GetActiveActivitiesAsync();
                return View(model);
            }

            TempData["SuccessMessage"] = "Терминот е успешно закажан!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailability(DateTime date, int activityId)
        {
            if (activityId == 0)
            {
                return BadRequest("Activity is required");
            }

            var availability = await _appointmentService.GetAvailabilityAsync(date, activityId);

            var response = new
            {
                date = availability.Date.ToString("yyyy-MM-dd"),
                activityId = availability.ActivityId,
                activityName = availability.ActivityName,
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
        [ValidateAntiForgeryToken]
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

        private async Task CleanupExpiredAppointmentsAsync()
        {
            try
            {
                var allAppointments = await _appointmentService.GetAllAppointmentsAsync();
                var expiredAppointments = allAppointments
                    .Where(a => a.EndTime < DateTime.Now && a.Status != "Cancelled")
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
    }
}