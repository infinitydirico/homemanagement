using HomeManagement.API.Business;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HomeManagement.API.Schedule
{
    public class NotificationGeneratorHostedService : HostedService
    {
        public NotificationGeneratorHostedService(ILogger<HostedService> logger,
            IServiceScopeFactory factory)
            : base(logger, factory)
        {
        }

        public override int GetPeriodToRun() => 30;

        public override void Process()
        {
            var notificationService = GetService<INotificationService>();
            notificationService.GenerateNotifications();
        }
    }
}
