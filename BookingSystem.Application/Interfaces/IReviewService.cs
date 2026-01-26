using BookingSystem.Application.DTOs;

namespace BookingSystem.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto?> GetReviewByIdAsync(int id);
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewDto>> GetReviewsByActivityIdAsync(int activityId);
        Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId);
        Task<ReviewDto> CreateReviewAsync(int userId, CreateReviewDto dto);
        Task<bool> UpdateReviewAsync(int id, int userId, CreateReviewDto dto);
        Task<bool> DeleteReviewAsync(int id, int userId);
        Task<double> GetAverageRatingAsync(int activityId);
    }
}