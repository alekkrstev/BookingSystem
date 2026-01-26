using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Domain.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(int id);
        Task<IEnumerable<Activity>> GetAllAsync();
        Task<IEnumerable<Activity>> GetActiveAsync();
        Task<Activity> AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}