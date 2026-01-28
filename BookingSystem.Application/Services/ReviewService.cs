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
        private readonly IAppointmentRepository _appointmentRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IActivityRepository activityRepository,
            IAppointmentRepository appointmentRepository)
        {
            _reviewRepository = reviewRepository;
            _activityRepository = activityRepository;
            _appointmentRepository = appointmentRepository;
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
            // Validate based on review type
            if (dto.ReviewType == "Activity")
            {
                if (!dto.ActivityId.HasValue)
                    throw new Exception("Activity is required for activity reviews");

                var activityExists = await _activityRepository.ExistsAsync(dto.ActivityId.Value);
                if (!activityExists)
                    throw new Exception("Activity not found");
            }

            var review = new Review
            {
                UserId = userId,
                ActivityId = dto.ActivityId,
                AppointmentId = dto.AppointmentId,
                ReviewType = dto.ReviewType,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _reviewRepository.AddAsync(review);

            // Mark appointment as reviewed if applicable
            // Mark appointment as reviewed if applicable
            if (dto.AppointmentId.HasValue && dto.AppointmentId.Value > 0)
            {
                try
                {
                    var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId.Value);
                    if (appointment != null)
                    {
                        appointment.HasReview = true;
                        await _appointmentRepository.UpdateAsync(appointment);
                    }
                }
                catch
                {
                    // Ignore if appointment not found
                }
            }

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
                ActivityName = review.ReviewType == "Playroom" ? "ODAX Playroom" : (review.Activity?.NameMk ?? "Unknown"),
                AppointmentId = review.AppointmentId,
                ReviewType = review.ReviewType,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewDto>> GetPlayroomReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return reviews
                .Where(r => r.ReviewType == "Playroom")
                .OrderByDescending(r => r.CreatedAt)
                .Select(MapToDto);
        }
    }
}