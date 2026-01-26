using BookingSystem.Domain.Interfaces;

namespace BookingSystem.Web.Services
{
    public class AppointmentCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentCleanupService> _logger;

        public AppointmentCleanupService(
            IServiceProvider serviceProvider,
            ILogger<AppointmentCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Appointment Cleanup Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupExpiredAppointments();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during appointment cleanup.");
                }

                // Провери на секои 1 час
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task CleanupExpiredAppointments()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var appointmentRepository = scope.ServiceProvider
                    .GetRequiredService<IAppointmentRepository>();

                var allAppointments = await appointmentRepository.GetAllAsync();
                var expiredAppointments = allAppointments
                    .Where(a => a.EndTime < DateTime.Now && a.Status != "Cancelled")
                    .ToList();

                foreach (var appointment in expiredAppointments)
                {
                    await appointmentRepository.DeleteAsync(appointment.Id);
                    _logger.LogInformation(
                        $"Deleted expired appointment: ID={appointment.Id}, " +
                        $"Service={appointment.ActivityId}, EndTime={appointment.EndTime}");
                }

                if (expiredAppointments.Any())
                {
                    _logger.LogInformation($"Cleaned up {expiredAppointments.Count} expired appointments.");
                }
            }
        }
    }
}