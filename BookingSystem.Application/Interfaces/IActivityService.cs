using BookingSystem.Application.DTOs;

namespace BookingSystem.Application.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityDto?> GetActivityByIdAsync(int id);
        Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync();
        Task<IEnumerable<ActivityDto>> GetActiveActivitiesAsync();
        Task<ActivityDto> CreateActivityAsync(CreateActivityDto dto);
        Task<bool> UpdateActivityAsync(int id, CreateActivityDto dto);
        Task<bool> DeleteActivityAsync(int id);
        Task<bool> ToggleActivityStatusAsync(int id);
    }
}