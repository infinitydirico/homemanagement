using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels.Main
{
    public class NotificationsViewModel : BaseViewModel
    {
        INotificationManager notificationManager = App._container.Resolve<INotificationManager>();

        public IEnumerable<NotificationModel> Notifications { get; private set; }

        public string TitleText => "Notifications";

        public async Task Dismiss(NotificationModel notificationModel)
        {
            await notificationManager.Dismiss(notificationModel);
            await InitializeAsync();
        }

        protected override async Task InitializeAsync()
        {
            Notifications = await notificationManager.GetNotifications();
            OnPropertyChanged(nameof(Notifications));
        }
    }
}
