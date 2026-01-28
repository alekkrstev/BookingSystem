using BookingSystem.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookingSystem.Web.Services
{
    public class AppointmentCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppointmentCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                        var appointments = await appointmentService.GetAllAppointmentsAsync();

                        // Брише само Cancelled и Pending завршени резервации
                        // НЕ брише Confirmed - за да може корисникот да остави review!
                        var expiredAppointments = appointments
                            .Where(a => a.EndTime < DateTime.Now &&
                                       (a.Status == "Cancelled" || a.Status == "Pending"))
                            .ToList();

                        foreach (var appointment in expiredAppointments)
                        {
                            await appointmentService.DeleteAppointmentAsync(appointment.Id);
                            Console.WriteLine($"[Cleanup] Deleted {appointment.Status} appointment #{appointment.Id}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Cleanup Error] {ex.Message}");
                }

                // Провери секој час
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}