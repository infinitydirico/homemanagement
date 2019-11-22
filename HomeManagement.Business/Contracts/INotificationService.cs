using HomeManagement.Models;
using System.Collections.Generic;

namespace HomeManagement.Business.Contracts
{
    public interface INotificationService
    {
        IEnumerable<NotificationModel> GetNotifications();

        void Dismiss(NotificationModel model);

        IEnumerable<ReminderModel> GetReminders();

        ReminderModel GetReminder(int id);

        OperationResult AddReminder(ReminderModel reminderModel);

        OperationResult UpdateReminder(ReminderModel reminderModel);

        OperationResult DeleteReminder(int id);
    }
}
