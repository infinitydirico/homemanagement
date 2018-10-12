using System.Collections.Generic;
using System.Linq;
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
    [Route("api/Reminder")]
    public class ReminderController : Controller
    {
        private readonly IReminderRepository reminderRepository;
        private readonly IUserRepository userRepository;
        private readonly IReminderMapper reminderMapper;

        public ReminderController(IReminderRepository reminderRepository,
            IUserRepository userRepository,
            IReminderMapper reminderMapper)
        {
            this.reminderRepository = reminderRepository;
            this.userRepository = userRepository;
            this.reminderMapper = reminderMapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var emailClaim = HttpContext.GetEmailClaim();

            var reminders = (from reminder in reminderRepository.All
                             join user in userRepository.All
                             on reminder.UserId equals user.Id
                             where user.Email.Equals(emailClaim.Value)
                             select reminderMapper.ToModel(reminder)).ToList();

            return Ok(reminders);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var emailClaim = HttpContext.GetEmailClaim();

            var reminderModel = (from reminder in reminderRepository.All
                             join user in userRepository.All
                             on reminder.UserId equals user.Id
                             where user.Email.Equals(emailClaim.Value)
                                    && reminder.Id.Equals(id)
                             select reminderMapper.ToModel(reminder)).FirstOrDefault();

            return Ok(reminderModel);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ReminderModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reminder = reminderMapper.ToEntity(model);

            if(reminder.UserId == 0)
            {
                reminder.UserId = GetUserId();
            }

            reminderRepository.Add(reminder);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] ReminderModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reminder = reminderMapper.ToEntity(model);

            if (reminder.UserId == 0)
            {
                reminder.UserId = GetUserId();
            }

            reminderRepository.Update(reminder);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var reminder = reminderRepository.GetById(id);

            var userId = GetUserId();

            if (reminder.UserId != userId) return Forbid();

            reminderRepository.Remove(reminder);

            return Ok();
        }

        private int GetUserId()
        {
            var emailClaim = HttpContext.GetEmailClaim();

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(emailClaim.Value));

            return user.Id;
        }
    }
}