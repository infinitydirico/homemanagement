using System;
using System.Threading.Tasks;

namespace HomeManagement.API.WebApp.Services.Notification
{
    public interface INotifier
    {
        Task RaiseNotification();

        event Func<Task> Notify;
    }
}
