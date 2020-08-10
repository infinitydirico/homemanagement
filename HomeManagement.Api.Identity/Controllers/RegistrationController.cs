using HomeManagement.API.Queue.Messages;
using HomeManagement.API.RabbitMQ;
using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using HomeManagement.Core.Extensions;

namespace HomeManagement.Api.Identity.Controllers
{
    [EnableCors("IdentityApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICryptography cryptography;
        private readonly IQueueService  queueService;

        public RegistrationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ICryptography cryptography,
            IQueueService queueService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.cryptography = cryptography;
            this.queueService = queueService;
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
                        var roles = roleManager.Roles.ToList();
                        var role = roles.First(x => x.Name.Equals("RegularUser"));
                        
                        await userManager.AddToRoleAsync(user, role.Name);

                        queueService.SendMessage(new RegistrationMessage
                        {
                            Email = userModel.Email,
                            Language = userModel.Language.IsEmpty() ? GetBrowserLanguage() : userModel.Language
                        });
                        scope.Complete();
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

        private string GetBrowserLanguage()
        {
            try
            {
                var acceptedLanguages = HttpContext.Request.GetTypedHeaders().AcceptLanguage;

                if (acceptedLanguages.Any(x => x.Quality.HasValue))
                {
                    var language = acceptedLanguages
                        .Where(x => x.Quality.HasValue)
                        .OrderBy(x => x.Quality)
                        .First();

                    return language.Value.Value;
                }
                else
                {
                    return acceptedLanguages.First().Value.Value;
                }
            }
            catch
            {
                return "en";
            }
        }
    }
}