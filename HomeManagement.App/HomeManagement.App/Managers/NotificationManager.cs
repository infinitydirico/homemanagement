using Autofac;
using HomeManagement.App.Services.Rest;
using HomeManagement.Core.Caching;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationServiceClient notificationServiceClient = App._container.Resolve<INotificationServiceClient>();
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();

        public async Task Dismiss(NotificationModel notificationModel)
        {
            notificationModel.Dismissed = true;
            await notificationServiceClient.UpdateNotification(notificationModel);

            cachingService.Remove(nameof(NotificationManager.GetNotifications));
        }

        public async Task<IEnumerable<NotificationModel>> GetNotifications()
        {
            if(cachingService.Exists(nameof(NotificationManager.GetNotifications)))
            {
                return cachingService.Get<IEnumerable<NotificationModel>>(nameof(NotificationManager.GetNotifications));
            }

            var notifications = await notificationServiceClient.GetNotifications();

            cachingService.Store(nameof(NotificationManager.GetNotifications), notifications);

            return notifications;
        }
    }

    public interface INotificationManager
    {
        Task<IEnumerable<NotificationModel>> GetNotifications();

        Task Dismiss(NotificationModel notificationModel);
    }
}
