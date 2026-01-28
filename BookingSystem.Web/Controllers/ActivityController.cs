using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        // GET: Activity/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var activities = await _activityService.GetAllActivitiesAsync();
            return View(activities);
        }

        // GET: Activity/Browse (за сите корисници)
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            var activities = await _activityService.GetActiveActivitiesAsync(); // Само активни
            return View(activities);
        }

        // GET: Activity/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activity/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateActivityDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _activityService.CreateActivityAsync(dto);
            TempData["SuccessMessage"] = "Активноста е успешно креирана!";
            return RedirectToAction("Index");
        }

        // GET: Activity/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null)
                return NotFound();

            var dto = new CreateActivityDto
            {
                Name = activity.Name,
                NameMk = activity.NameMk,
                Description = activity.Description,
                Icon = activity.Icon,
                PricePerHour = activity.PricePerHour,
                MaxPlayers = activity.MaxPlayers,
                IsActive = activity.IsActive
            };

            return View(dto);
        }

        // POST: Activity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateActivityDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _activityService.UpdateActivityAsync(id, dto);
            if (!result)
                return NotFound();

            TempData["SuccessMessage"] = "Активноста е успешно ажурирана!";
            return RedirectToAction("Index");
        }

        // POST: Activity/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _activityService.ToggleActivityStatusAsync(id);
            if (!result)
                return NotFound();

            TempData["SuccessMessage"] = "Статусот на активноста е променет!";
            return RedirectToAction("Index");
        }

        // POST: Activity/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _activityService.DeleteActivityAsync(id);
            if (!result)
                return NotFound();

            TempData["SuccessMessage"] = "Активноста е избришана!";
            return RedirectToAction("Index");
        }
    }
}