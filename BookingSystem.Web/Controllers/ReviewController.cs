using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingSystem.Web.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IActivityService _activityService;

        public ReviewController(
            IReviewService reviewService,
            IActivityService activityService)
        {
            _reviewService = reviewService;
            _activityService = activityService;
        }

        // GET: Review/Create?activityId=1
        [HttpGet]
        public async Task<IActionResult> Create(int activityId)
        {
            var activity = await _activityService.GetActivityByIdAsync(activityId);
            if (activity == null)
                return NotFound();

            ViewBag.Activity = activity;

            var model = new CreateReviewDto
            {
                ActivityId = activityId
            };

            return View(model);
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Load activity only if it's an activity review
                if (dto.ActivityId.HasValue)
                {
                    var activity = await _activityService.GetActivityByIdAsync(dto.ActivityId.Value);
                    ViewBag.Activity = activity;
                }
                return View(dto);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                await _reviewService.CreateReviewAsync(userId, dto);
                TempData["SuccessMessage"] = "Вашата рецензија е успешно додадена!";
                return RedirectToAction("MyReviews");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                // Load activity only if it's an activity review
                if (dto.ActivityId.HasValue)
                {
                    var activity = await _activityService.GetActivityByIdAsync(dto.ActivityId.Value);
                    ViewBag.Activity = activity;
                }

                return View(dto);
            }
        }

        // GET: Review/MyReviews
        [HttpGet]
        public async Task<IActionResult> MyReviews()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
            return View(reviews);
        }

        // GET: Review/ActivityReviews/1
        [HttpGet]
        public async Task<IActionResult> ActivityReviews(int id)
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null)
                return NotFound();

            ViewBag.Activity = activity;
            var reviews = await _reviewService.GetReviewsByActivityIdAsync(id);
            return View(reviews);
        }

        // POST: Review/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _reviewService.DeleteReviewAsync(id, userId);

            if (!result)
                return NotFound();

            TempData["SuccessMessage"] = "Рецензијата е избришана!";
            return RedirectToAction("MyReviews");
        }

        // GET: Review/CreatePlayroomReview
        [HttpGet]
        public IActionResult CreatePlayroomReview()
        {
            var model = new CreateReviewDto
            {
                ReviewType = "Playroom"
            };

            ViewBag.IsPlayroomReview = true;
            return View("Create", model);
        }

        // GET: Review/PlayroomReviews
        [AllowAnonymous] // Сите можат да гледаат
        [HttpGet]
        public async Task<IActionResult> PlayroomReviews()
        {
            var reviews = await _reviewService.GetPlayroomReviewsAsync();
            return View(reviews);
        }
    }
}