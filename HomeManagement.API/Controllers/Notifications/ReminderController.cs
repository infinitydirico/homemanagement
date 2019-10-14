using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
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
            return Ok(notificationService.GetReminders());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(notificationService.GetReminder(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] ReminderModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            notificationService.AddReminder(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] ReminderModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            notificationService.UpdateReminder(model);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();

            notificationService.DeleteReminder(id);

            return Ok();
        }
    }
}