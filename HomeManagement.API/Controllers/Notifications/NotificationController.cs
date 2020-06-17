using HomeManagement.Api.Core.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Business.Contracts;
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
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var claim = HttpContext.GetEmailClaim();

            return Ok(notificationService.GetNotifications());
        }

        [HttpPut]
        public IActionResult Put([FromBody]NotificationModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            notificationService.Dismiss(model);

            return Ok();
        }
    }
}
