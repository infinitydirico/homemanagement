using System;
using System.Threading.Tasks;

namespace HomeManagement.API.WebApp.Services.Notification
{
    public class NewTransactionAddedNotification : INotifier
    {
        public event Func<Task> Notify;

        public async Task RaiseNotification()
        {
            if(Notify != null)
            {
                await Notify.Invoke();
            }            
        }
    }
}
