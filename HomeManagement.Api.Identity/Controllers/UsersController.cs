using HomeManagement.Api.Identity.Filters;
using HomeManagement.API.RabbitMQ;
using HomeManagement.API.RabbitMQ.Messages;
using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AdminAuthorization]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<UsersController> logger;
        private readonly IQueueService queueService;

        public UsersController(
            UserManager<IdentityUser> userManager,
            ILogger<UsersController> logger,
            IQueueService queueService)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.queueService = queueService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await userManager
                .Users
                .Select(x => new UserIdentityModel { Id = x.Id, Email = x.Email })
                .ToListAsync();

            return Ok(users);
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                var user = await userManager.FindByNameAsync(email);

                var userRoles = await userManager.GetRolesAsync(user);

                if (userRoles.Any(x => x.Equals("Administrator")))
                {
                    return BadRequest("Can't remove an admin.");
                }

                await userManager.DeleteAsync(user);
                queueService.SendMessage(new DeleteUserMessage
                {
                    Email = email
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unable to delete user: {email}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}
