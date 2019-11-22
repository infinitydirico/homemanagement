using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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
            var repositoryFactory = GetService<IRepositoryFactory>();

            using (var reminderRepository = repositoryFactory.CreateReminderRepository())
            using (var notificationRepository = repositoryFactory.CreateNotificationRepository())
            {
                var reminders = reminderRepository.GetAll();

                var notifications = notificationRepository
                    .Where(n => n.CreatedOn < DateTime.Now &&
                                n.CreatedOn.Month.Equals(DateTime.Now.Month));

                if (notifications.Count() > 0 && reminders.Count().Equals(notifications.Count())) return;

                foreach (var reminder in reminders.Where(x => !notifications.Any(y => y.ReminderId.Equals(x.Id))))
                {
                    var notification = new Notification
                    {
                        ReminderId = reminder.Id,
                        CreatedOn = DateTime.Now,
                        Dismissed = false,
                    };

                    notificationRepository.Add(notification);
                }
                notificationRepository.Commit();
            }
        }
    }
}
