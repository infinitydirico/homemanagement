using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public interface INotificationMapper : IMapper<Notification, NotificationModel>
    {

    }

    public class NotificationMapper : BaseMapper<Notification, NotificationModel>, INotificationMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(Account).GetProperty(nameof(Notification.Id));
            yield return typeof(Account).GetProperty(nameof(Notification.ReminderId));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(Account).GetProperty(nameof(NotificationModel.Id));
            yield return typeof(Account).GetProperty(nameof(NotificationModel.ReminderId));
            yield return typeof(Account).GetProperty(nameof(NotificationModel.Title));
            yield return typeof(Account).GetProperty(nameof(NotificationModel.Description));
            yield return typeof(Account).GetProperty(nameof(NotificationModel.DueDay));
        }
    }
}
