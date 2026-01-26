using BookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteService _quoteService;

        public HomeController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        public async Task<IActionResult> Index()
        {
            // Get daily motivational quote
            var quote = await _quoteService.GetRandomQuoteAsync();
            ViewBag.DailyQuote = quote;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}