using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest
{
    public class NotificationServiceClient
    {
        BaseRestClient restClient;

        public NotificationServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.BASEURL);
        }

        public async Task<IEnumerable<NotificationModel>> GetNotifications()
        {
            var api = $"{Endpoints.Notifications.Notification}";
            var result = await restClient.Get<IEnumerable<NotificationModel>>(api);
            return result;
        }

        public async Task UpdateNotification(NotificationModel notificationModel)
        {
            var api = $"{Endpoints.Notifications.Notification}";
            await restClient.Put(api, notificationModel);
        }
    }
}
