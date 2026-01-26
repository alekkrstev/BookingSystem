using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;

namespace BookingSystem.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IActivityRepository _activityRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IActivityRepository activityRepository)
        {
            _reviewRepository = reviewRepository;
            _activityRepository = activityRepository;
        }

        public async Task<ReviewDto?> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return review == null ? null : MapToDto(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return reviews.Select(MapToDto);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByActivityIdAsync(int activityId)
        {
            var reviews = await _reviewRepository.GetByActivityIdAsync(activityId);
            return reviews.Select(MapToDto);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId)
        {
            var reviews = await _reviewRepository.GetByUserIdAsync(userId);
            return reviews.Select(MapToDto);
        }

        public async Task<ReviewDto> CreateReviewAsync(int userId, CreateReviewDto dto)
        {
            // Verify activity exists
            var activityExists = await _activityRepository.ExistsAsync(dto.ActivityId);
            if (!activityExists)
                throw new Exception("Activity not found");

            var review = new Review
            {
                UserId = userId,
                ActivityId = dto.ActivityId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _reviewRepository.AddAsync(review);
            return MapToDto(created);
        }

        public async Task<bool> UpdateReviewAsync(int id, int userId, CreateReviewDto dto)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null || review.UserId != userId)
                return false;

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            await _reviewRepository.UpdateAsync(review);
            return true;
        }

        public async Task<bool> DeleteReviewAsync(int id, int userId)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null || review.UserId != userId)
                return false;

            await _reviewRepository.DeleteAsync(id);
            return true;
        }

        public async Task<double> GetAverageRatingAsync(int activityId)
        {
            return await _reviewRepository.GetAverageRatingAsync(activityId);
        }

        private ReviewDto MapToDto(Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                UserName = review.User?.UserName ?? "Unknown",
                ActivityId = review.ActivityId,
                ActivityName = review.Activity?.NameMk ?? "Unknown",
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}