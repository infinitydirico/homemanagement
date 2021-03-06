﻿using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Business.Units
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly INotificationMapper notificationMapper;
        private readonly IReminderMapper reminderMapper;
        private readonly IUserSessionService userService;

        public NotificationService(IRepositoryFactory repositoryFactory,
            INotificationMapper notificationMapper,
            IReminderMapper reminderMapper,
            IUserSessionService userService)
        {
            this.repositoryFactory = repositoryFactory;
            this.notificationMapper = notificationMapper;
            this.reminderMapper = reminderMapper;
            this.userService = userService;
        }

        public OperationResult AddReminder(ReminderModel reminderModel)
        {
            using (var reminderRepository = repositoryFactory.CreateReminderRepository())
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
        }

        public OperationResult DeleteReminder(int id)
        {
            using (var reminderRepository = repositoryFactory.CreateReminderRepository())
            {
                var authenticatedUser = userService.GetAuthenticatedUser();

                var reminder = reminderRepository.GetById(id);

                var userId = authenticatedUser.Id;

                if (reminder.UserId != userId) return OperationResult.Error("UserId does not match");

                reminderRepository.Remove(reminder);
                reminderRepository.Commit();

                return OperationResult.Succeed();
            }
        }

        public void Dismiss(NotificationModel model)
        {
            using (var notificationRepository = repositoryFactory.CreateNotificationRepository())
            {
                var notification = notificationRepository.GetById(model.Id);

                notification.Dismissed = model.Dismissed;

                notificationRepository.Update(notification);
                notificationRepository.Commit();
            }
        }

        public IEnumerable<NotificationModel> GetNotifications()
        {
            var notificationRepository = repositoryFactory.CreateNotificationRepository();

            var authenticatedUser = userService.GetAuthenticatedUser();

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
            using (var reminderRepository = repositoryFactory.CreateReminderRepository())
            {
                var authenticatedUser = userService.GetAuthenticatedUser();

                var reminder = reminderRepository.FirstOrDefault(r => r.Id.Equals(id));

                return reminderMapper.ToModel(reminder);
            }
        }

        public IEnumerable<ReminderModel> GetReminders()
        {
            using (var reminderRepository = repositoryFactory.CreateReminderRepository())
            {
                var authenticatedUser = userService.GetAuthenticatedUser();

                var reminders = reminderRepository
                    .Where(r => r.User.Email.Equals(authenticatedUser.Email))
                    .Select(r => reminderMapper.ToModel(r))
                    .ToList();

                return reminders;
            }
        }

        public OperationResult UpdateReminder(ReminderModel reminderModel)
        {
            using (var reminderRepository = repositoryFactory.CreateReminderRepository())
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
        }
    }
}
