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
            yield return typeof(Notification).GetProperty(nameof(Notification.Id));
            yield return typeof(Notification).GetProperty(nameof(Notification.ReminderId));
            yield return typeof(Notification).GetProperty(nameof(Notification.Dismissed));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(NotificationModel).GetProperty(nameof(NotificationModel.Id));
            yield return typeof(NotificationModel).GetProperty(nameof(NotificationModel.ReminderId));
            yield return typeof(NotificationModel).GetProperty(nameof(NotificationModel.Title));
            yield return typeof(NotificationModel).GetProperty(nameof(NotificationModel.Description));
            yield return typeof(NotificationModel).GetProperty(nameof(NotificationModel.DueDay));
            yield return typeof(NotificationModel).GetProperty(nameof(NotificationModel.Dismissed));
        }
    }
}
