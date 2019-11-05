using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Data
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Notification entity) => GetById(entity.Id) != null;

        public override Notification GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));

        public IEnumerable<Notification> GetPendingNotifications(int userId)
        {
            var reminderSet = platformContext.GetDbContext().Set<Reminder>();

            var notificationSet = platformContext.GetDbContext().Set<Notification>();

            var notifications = from n in notificationSet
                                join r in reminderSet
                                on n.ReminderId equals r.Id
                                where r.UserId.Equals(userId) &&
                                        !n.Dismissed &&
                                        n.CreatedOn.Month.Equals(DateTime.Now.Month)
                                select new Notification
                                {
                                    Id = n.Id,
                                    CreatedOn = n.CreatedOn,
                                    Dismissed = false,
                                    ReminderId = n.ReminderId,
                                    Reminder = r
                                };

            return notifications.ToList();
        }
    }
}
