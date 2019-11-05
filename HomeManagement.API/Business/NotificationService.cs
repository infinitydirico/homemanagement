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
        private readonly IUserSessionService userService;

        public NotificationService(IReminderRepository reminderRepository,
            IUserRepository userRepository,
            INotificationMapper notificationMapper,
            INotificationRepository notificationRepository,
            IReminderMapper reminderMapper,
            IUserSessionService userService)
        {
            this.reminderRepository = reminderRepository;
            this.userRepository = userRepository;
            this.notificationMapper = notificationMapper;
            this.notificationRepository = notificationRepository;
            this.reminderMapper = reminderMapper;
            this.userService = userService;
        }

        public OperationResult AddReminder(ReminderModel reminderModel)
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var reminder = reminderMapper.ToEntity(reminderModel);

            if (reminder.UserId == 0)
            {
                reminder.UserId = authenticatedUser.Id;
            }

            reminderRepository.Add(reminder);
            reminderRepository.Commit();

            return OperationResult.Succeed();
        }

        public OperationResult DeleteReminder(int id)
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var reminder = reminderRepository.GetById(id);

            var userId = authenticatedUser.Id;

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
            reminderRepository.Commit();
        }

        public IEnumerable<NotificationModel> GetNotifications()
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            GenerateNotifications(authenticatedUser.Email);

            var notifications = notificationRepository
                .GetPendingNotifications(authenticatedUser.Id)
                .Select(x => new NotificationModel
                {
                    Id = x.Id,
                    Title = x.Reminder.Title,
                    Description = x.Reminder.Description,
                    Dismissed = false,
                    DueDay = 5,
                    ReminderId = x.ReminderId
                })
                .ToList();

            return notifications;
        }

        public ReminderModel GetReminder(int id)
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var reminder = reminderRepository.FirstOrDefault(r => r.Id.Equals(id));

            return reminderMapper.ToModel(reminder);
        }

        public IEnumerable<ReminderModel> GetReminders()
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var reminders = reminderRepository
                .Where(r => r.User.Email.Equals(authenticatedUser.Email))
                .Select(r => reminderMapper.ToModel(r))
                .ToList();

            return reminders;
        }

        public OperationResult UpdateReminder(ReminderModel reminderModel)
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var reminder = reminderMapper.ToEntity(reminderModel);

            if (reminder.UserId == 0)
            {
                reminder.UserId = authenticatedUser.Id;
            }

            reminderRepository.Update(reminder);
            reminderRepository.Commit();

            return OperationResult.Succeed();
        }

        private void GenerateNotifications(string email)
        {
            var reminders = reminderRepository.GetUserActiveReminders(email);

            var notifications = notificationRepository
                .Where(n => n.Reminder.User.Email.Equals(email) &&
                            n.CreatedOn < DateTime.Now &&
                            n.CreatedOn.Month.Equals(DateTime.Now.Month));

            if (notifications.Count() > 0 && reminders.Count().Equals(notifications.Count())) return;

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
            reminderRepository.Commit();
        }
    }

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
