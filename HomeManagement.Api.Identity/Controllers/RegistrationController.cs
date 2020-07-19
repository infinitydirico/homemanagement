using HomeManagement.Api.Identity.Services;
using HomeManagement.API.Queue.Messages;
using HomeManagement.API.RabbitMQ;
using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HomeManagement.Api.Identity.Controllers
{
    [EnableCors("IdentityApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ICryptography cryptography;
        private readonly IBroadcaster broadcaster;
        private readonly IConfiguration configuration;

        public RegistrationController(UserManager<IdentityUser> userManager,
            ICryptography cryptography,
            IBroadcaster broadcaster,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.cryptography = cryptography;
            this.broadcaster = broadcaster;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Select(x => x.Value));

            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var password = cryptography.Decrypt(userModel.Password);
                    var user = new IdentityUser
                    {
                        Email = userModel.Email,
                        UserName = userModel.Email
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        //broadcaster.BroadcastRegistration(userModel.Email, userModel.Language);
                        //scope.Complete();
                        var sender = new Sender(configuration);
                        sender.SendMessage(new RegistrationMessage
                        {
                            Email = userModel.Email,
                            Language = userModel.Language
                        });
                        return Ok();
                    }
                    else
                    {
                        scope.Dispose();
                        return BadRequest(result.Errors);
                    }

                }
                catch (System.Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }
    }
}