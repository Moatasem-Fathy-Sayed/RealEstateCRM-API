using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateCRM.Infrastructure.Services
{
    public class AppointmentReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentReminderService> _logger;

        public AppointmentReminderService(IServiceProvider serviceProvider, ILogger<AppointmentReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking upcoming appointments for reminders...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var todayAppointments = dbContext.Appointments
                        .Where(a => a.AppointmentDate.Date == DateTime.UtcNow.Date && a.Status == Core.Enums.AppointmentStatus.Scheduled)
                        .ToList();

                    foreach (var appointment in todayAppointments)
                    {
                        bool exists = dbContext.Notifications.Any(n => n.Message.Contains($"Appointment ID {appointment.Id}"));
                        if (!exists)
                        {
                            dbContext.Notifications.Add(new Notification
                            {
                                Title = "Appointment Reminder",
                                Message = $"Reminder: You have a property viewing appointment (ID {appointment.Id}) scheduled for today.",
                                IsRead = false,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                // Wait 1 hour before checking again
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}