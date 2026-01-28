using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Seed Activities first
            await SeedActivitiesAsync();

            // Seed Users
            await SeedUsersAsync();

            // Seed Appointments (optional - можеш да го избришеш ова)
            // await SeedAppointmentsAsync();
        }

        private async Task SeedActivitiesAsync()
        {
            if (await _context.Activities.AnyAsync())
                return; // Already seeded

            var activities = new List<Activity>
            {
                new Activity
                {
                    Name = "Sony PlayStation",
                    NameMk = "Sony PlayStation",
                    Icon = "🎮",
                    PricePerHour = 500,
                    MaxPlayers = 4,
                    IsActive = true
                },
                new Activity
                {
                    Name = "Darts",
                    NameMk = "Пикадо",
                    Icon = "🎯",
                    PricePerHour = 300,
                    MaxPlayers = 4,
                    IsActive = true
                },
                new Activity
                {
                    Name = "8-Ball Pool",
                    NameMk = "Билјард",
                    Icon = "🎱",
                    PricePerHour = 400,
                    MaxPlayers = 2,
                    IsActive = true
                },
                new Activity
                {
                    Name = "FIFA",
                    NameMk = "Фудбалче",
                    Icon = "⚽",
                    PricePerHour = 250,
                    MaxPlayers = 2,
                    IsActive = true
                }
            };

            await _context.Activities.AddRangeAsync(activities);
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsersAsync()
        {
            if (await _context.Users.AnyAsync())
                return; // Already seeded

            var users = new List<User>
            {
                new User
                {
                    UserName = "admin",
                    Email = "admin@booking.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    FirstName = "Admin",
                    LastName = "User",
                    Role = "Admin"
                },
                new User
                {
                    UserName = "testuser",
                    Email = "test@booking.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
                    FirstName = "Test",
                    LastName = "User",
                    Role = "User"
                }
            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }
    }
}