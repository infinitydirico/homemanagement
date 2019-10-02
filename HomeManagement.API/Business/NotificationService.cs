using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class NotificationService : INotificationService
    {
        private readonly IReminderRepository reminderRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly INotificationMapper notificationMapper;
        private readonly IReminderMapper reminderMapper;

        public NotificationService(IReminderRepository reminderRepository,
            IUserRepository userRepository,
            INotificationMapper notificationMapper,
            INotificationRepository notificationRepository,
            IReminderMapper reminderMapper)
        {
            this.reminderRepository = reminderRepository;
            this.userRepository = userRepository;
            this.notificationMapper = notificationMapper;
            this.notificationRepository = notificationRepository;
            this.reminderMapper = reminderMapper;
        }

        public OperationResult AddReminder(ReminderModel reminderModel, string email)
        {
            var reminder = reminderMapper.ToEntity(reminderModel);

            if (reminder.UserId == 0)
            {
                reminder.UserId = GetUserId(email);
            }

            reminderRepository.Add(reminder);
            reminderRepository.Commit();

            return OperationResult.Succeed();
        }

        public OperationResult DeleteReminder(int id, string email)
        {
            var reminder = reminderRepository.GetById(id);

            var userId = GetUserId(email);

            if (reminder.UserId != userId) return OperationResult.Error("UserId does not match");

            reminderRepository.Remove(reminder);
            reminderRepository.Commit();

            return OperationResult.Succeed();
        }

        public void Dismiss(NotificationModel model)
        {
            var notification = notificationRepository.GetById(model.Id);

            notification.Dismissed = model.Dismissed;

            notificationRepository.Update(notification);
            notificationRepository.Commit();
        }

        public IEnumerable<NotificationModel> GetNotifications(string email)
        {
            GenerateNotifications(email);

            var notifications = (from notification in notificationRepository.All
                                 join reminder in reminderRepository.All
                                 on notification.ReminderId equals reminder.Id
                                 join user in userRepository.All
                                 on reminder.UserId equals user.Id
                                 where user.Email.Equals(email)
                                        && !notification.Dismissed
                                        && notification.CreatedOn.Month.Equals(DateTime.Now.Month)
                                 select new NotificationModel
                                 {
                                     Id = notification.Id,
                                     ReminderId = notification.ReminderId,
                                     Title = reminder.Title,
                                     Description = reminder.Description,
                                     Dismissed = notification.Dismissed,
                                     DueDay = 5
                                 })
                                 .ToList();

            return notifications;
        }

        public ReminderModel GetReminder(int id, string email)
        {
            var reminderModel = (from reminder in reminderRepository.All
                                 join user in userRepository.All
                                 on reminder.UserId equals user.Id
                                 where user.Email.Equals(email)
                                        && reminder.Id.Equals(id)
                                 select reminderMapper.ToModel(reminder)).FirstOrDefault();

            return reminderModel;
        }

        public IEnumerable<ReminderModel> GetReminders(string email)
        {
            var reminders = (from reminder in reminderRepository.All
                             join user in userRepository.All
                             on reminder.UserId equals user.Id
                             where user.Email.Equals(email)
                             select reminderMapper.ToModel(reminder)).ToList();

            return reminders;
        }

        public OperationResult UpdateReminder(ReminderModel reminderModel, string email)
        {
            var reminder = reminderMapper.ToEntity(reminderModel);

            if (reminder.UserId == 0)
            {
                reminder.UserId = GetUserId(email);
            }

            reminderRepository.Update(reminder);
            reminderRepository.Commit();

            return OperationResult.Succeed();
        }

        private void GenerateNotifications(string email)
        {
            var reminders = (from reminder in reminderRepository.All
                             join user in userRepository.All
                             on reminder.UserId equals user.Id
                             where user.Email.Equals(email)
                                    && reminder.Active
                             select reminder)
                                 .ToList();

            var notifications = (from notification in notificationRepository.All
                                 join reminder in reminders
                                 on notification.ReminderId equals reminder.Id
                                 where notification.CreatedOn < DateTime.Now
                                       && notification.CreatedOn.Month.Equals(DateTime.Now.Month)
                                 select notification).ToList();

            if (notifications.Count > 0 && reminders.Count.Equals(notifications.Count)) return;

            foreach (var reminder in reminders.Where(x => !notifications.Any(y => y.ReminderId.Equals(x.Id))))
            {
                var notification = new Notification
                {
                    ReminderId = reminder.Id,
                    CreatedOn = DateTime.Now,
                    Dismissed = false,
                };

                notificationRepository.Add(notification);
            }
            notificationRepository.Commit();
        }

        private int GetUserId(string email)
        {
            var user = userRepository.FirstOrDefault(x => x.Email.Equals(email));

            return user.Id;
        }
    }

    public interface INotificationService
    {
        IEnumerable<NotificationModel> GetNotifications(string email);

        void Dismiss(NotificationModel model);

        IEnumerable<ReminderModel> GetReminders(string email);

        ReminderModel GetReminder(int id, string email);

        OperationResult AddReminder(ReminderModel reminderModel, string email);

        OperationResult UpdateReminder(ReminderModel reminderModel, string email);

        OperationResult DeleteReminder(int id, string email);
    }
}
