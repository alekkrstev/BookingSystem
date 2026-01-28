using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;

namespace BookingSystem.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IReviewRepository _reviewRepository;

        public ActivityService(
            IActivityRepository activityRepository,
            IReviewRepository reviewRepository)
        {
            _activityRepository = activityRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity == null) return null;

            return await MapToDto(activity);
        }

        public async Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync()
        {
            var activities = await _activityRepository.GetAllAsync();
            var dtos = new List<ActivityDto>();

            foreach (var activity in activities)
            {
                dtos.Add(await MapToDto(activity));
            }

            return dtos;
        }

        public async Task<IEnumerable<ActivityDto>> GetActiveActivitiesAsync()
        {
            var activities = await _activityRepository.GetActiveAsync();
            var dtos = new List<ActivityDto>();

            foreach (var activity in activities)
            {
                dtos.Add(await MapToDto(activity));
            }

            return dtos;
        }

        public async Task<ActivityDto> CreateActivityAsync(CreateActivityDto dto)
        {
            var activity = new Activity
            {
                Name = dto.Name,
                NameMk = dto.NameMk,
                Icon = dto.Icon,
                PricePerHour = dto.PricePerHour,
                MaxPlayers = dto.MaxPlayers,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _activityRepository.AddAsync(activity);
            return await MapToDto(created);
        }

        public async Task<bool> UpdateActivityAsync(int id, CreateActivityDto dto)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity == null) return false;

            activity.Name = dto.Name;
            activity.NameMk = dto.NameMk;
            activity.Icon = dto.Icon;
            activity.PricePerHour = dto.PricePerHour;
            activity.MaxPlayers = dto.MaxPlayers;
            activity.IsActive = dto.IsActive;

            await _activityRepository.UpdateAsync(activity);
            return true;
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            var exists = await _activityRepository.ExistsAsync(id);
            if (!exists) return false;

            await _activityRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ToggleActivityStatusAsync(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity == null) return false;

            activity.IsActive = !activity.IsActive;
            await _activityRepository.UpdateAsync(activity);
            return true;
        }

        private async Task<ActivityDto> MapToDto(Activity activity)
        {
            var averageRating = await _reviewRepository.GetAverageRatingAsync(activity.Id);

            return new ActivityDto
            {
                Id = activity.Id,
                Name = activity.Name,
                NameMk = activity.NameMk,
                Icon = activity.Icon,
                PricePerHour = activity.PricePerHour,
                MaxPlayers = activity.MaxPlayers,
                IsActive = activity.IsActive,
                CreatedAt = activity.CreatedAt,
                AverageRating = averageRating,
                ReviewCount = activity.Reviews?.Count ?? 0,
                AppointmentCount = activity.Appointments?.Count ?? 0
            };
        }
    }
}