using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System.Collections.Generic;

namespace HomeManagement.Data
{
    public interface INotificationRepository : IRepository<Notification>
    {
        IEnumerable<Notification> GetPendingNotifications(int userId);
    }
}
