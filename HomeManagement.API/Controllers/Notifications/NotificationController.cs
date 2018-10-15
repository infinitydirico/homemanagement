using System;
using System.Collections.Generic;
using System.Linq;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Notifications
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Notification")]
    public class NotificationController : Controller
    {
        private readonly IReminderRepository reminderRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly INotificationMapper notificationMapper;

        public NotificationController(IReminderRepository reminderRepository,
            IUserRepository userRepository,
            INotificationMapper notificationMapper,
            INotificationRepository notificationRepository)
        {
            this.reminderRepository = reminderRepository;
            this.userRepository = userRepository;
            this.notificationMapper = notificationMapper;
            this.notificationRepository = notificationRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            GenerateNotifications();

            var claim = HttpContext.GetEmailClaim();

            var notifications = (from notification in notificationRepository.All
                                 join reminder in reminderRepository.All
                                 on notification.ReminderId equals reminder.Id
                                 join user in userRepository.All
                                 on reminder.UserId equals user.Id
                                 where user.Email.Equals(claim.Value)
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

            return Ok(notifications);
        }

        [HttpPut]
        public void Put([FromBody]NotificationModel model)
        {
            var notification = notificationRepository.GetById(model.Id);

            notification.Dismissed = model.Dismissed;

            notificationRepository.Update(notification);
        }

        private void GenerateNotifications()
        {
            var claim = HttpContext.GetEmailClaim();

            var reminders = (from reminder in reminderRepository.All
                                 join user in userRepository.All
                                 on reminder.UserId equals user.Id
                                 where user.Email.Equals(claim.Value)
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
        }
    }
}
