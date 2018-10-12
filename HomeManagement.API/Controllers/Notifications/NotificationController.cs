using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
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
            var claim = HttpContext.GetEmailClaim();

            var notifications = (from notification in notificationRepository.All
                                 join reminder in reminderRepository.All
                                 on notification.ReminderId equals reminder.Id
                                 join user in userRepository.All
                                 on reminder.UserId equals user.Id
                                 where user.Email.Equals(claim.Value)
                                        && !notification.Dismissed
                                 select notificationMapper.ToModel(notification))
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
    }
}
