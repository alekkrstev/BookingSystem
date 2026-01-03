using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;

namespace BookingSystem.Infrastructure.Seeders
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
            // Check if users already exist
            if (_context.Users.Any())
                return;

            // Create admin user
            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@bookingsystem.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                FirstName = "Админ",
                LastName = "Администратор",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            // Create test user
            var testUser = new User
            {
                UserName = "testuser",
                Email = "test@bookingsystem.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
                FirstName = "Тест",
                LastName = "Корисник",
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.AddRange(adminUser, testUser);
            await _context.SaveChangesAsync();

            // Create some sample appointments for test user
            var appointment1 = new Appointment
            {
                UserId = testUser.Id,
                ServiceType = "Sony PlayStation",
                StartTime = DateTime.Now.AddDays(1).Date.AddHours(20),
                EndTime = DateTime.Now.AddDays(1).Date.AddHours(21),
                Status = "Pending",
                Notes = "GTA V игра",
                CreatedAt = DateTime.UtcNow
            };

            var appointment2 = new Appointment
            {
                UserId = testUser.Id,
                ServiceType = "Darts",
                StartTime = DateTime.Now.AddDays(2).Date.AddHours(19).AddMinutes(30),
                EndTime = DateTime.Now.AddDays(2).Date.AddHours(20).AddMinutes(30),
                Status = "Confirmed",
                Notes = "Турнир во дартс",
                CreatedAt = DateTime.UtcNow
            };

            var appointment3 = new Appointment
            {
                UserId = testUser.Id,
                ServiceType = "8-Ball Pool",
                StartTime = DateTime.Now.AddDays(3).Date.AddHours(21),
                EndTime = DateTime.Now.AddDays(3).Date.AddHours(22),
                Status = "Pending",
                Notes = "Пријателски меч",
                CreatedAt = DateTime.UtcNow
            };

            _context.Appointments.AddRange(appointment1, appointment2, appointment3);
            await _context.SaveChangesAsync();
        }
    }
}