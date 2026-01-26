using BookingSystem.Application.DTOs;

namespace BookingSystem.Application.Interfaces
{
    public interface IQuoteService
    {
        Task<QuoteDto?> GetRandomQuoteAsync();
        Task<QuoteDto?> GetActivityMotivationAsync(string activityName);
    }
}