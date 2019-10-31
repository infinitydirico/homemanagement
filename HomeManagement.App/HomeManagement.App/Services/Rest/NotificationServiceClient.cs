using HomeManagement.App.Common;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class NotificationServiceClient : INotificationServiceClient
    {
        public async Task<IEnumerable<NotificationModel>> GetNotifications()
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync($"{Constants.Endpoints.Notifications.Notification}")
                .ReadContent<IEnumerable<NotificationModel>>();
        }

        public async Task UpdateNotification(NotificationModel notificationModel)
        {
            var result = await RestClientFactory
                .CreateAuthenticatedClient()
                .PutAsync($"{Constants.Endpoints.Notifications.Notification}", notificationModel.SerializeToJson());

            result.EnsureSuccessStatusCode();
        }
    }

    public interface INotificationServiceClient
    {
        Task<IEnumerable<NotificationModel>> GetNotifications();

        Task UpdateNotification(NotificationModel notificationModel);
    }
}
