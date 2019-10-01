using HomeManagement.API.Business;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Notifications
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Reminder")]
    public class ReminderController : Controller
    {
        private readonly INotificationService notificationService;

        public ReminderController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var emailClaim = HttpContext.GetEmailClaim();

            return Ok(notificationService.GetReminders(emailClaim.Value));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var emailClaim = HttpContext.GetEmailClaim();

            return Ok(notificationService.GetReminder(id, emailClaim.Value));
        }

        [HttpPost]
        public IActionResult Post([FromBody] ReminderModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            notificationService.AddReminder(model, HttpContext.GetEmailClaim().Value);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] ReminderModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            notificationService.UpdateReminder(model, HttpContext.GetEmailClaim().Value);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();

            notificationService.Delete(id, HttpContext.GetEmailClaim().Value);

            return Ok();
        }
    }
}