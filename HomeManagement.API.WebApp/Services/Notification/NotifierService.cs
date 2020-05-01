using System.Threading.Tasks;

namespace HomeManagement.API.WebApp.Services.Notification
{
    public class NotifierService
    {
        public async Task Update(INotifier notifier)
        {
            await notifier?.RaiseNotification();
        }
    }
}
