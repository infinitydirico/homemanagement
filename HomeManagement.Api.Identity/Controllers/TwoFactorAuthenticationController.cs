using HomeManagement.Api.Core.Extensions;
using HomeManagement.Api.Identity.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Controllers
{
    [EnableCors("IdentityApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFactorAuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public TwoFactorAuthenticationController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet("IsEnabled/{email}")]
        public async Task<IActionResult> IsEnabled(string email)
        {
            var appUser = await userManager.FindByEmailAsync(email);

            if (appUser == null) return Ok(false);

            var result = await userManager.GetTwoFactorEnabledAsync(appUser);

            return Ok(result);
        }

        [Authorization]
        [HttpGet("IsEnabled")]
        public async Task<IActionResult> IsEnabled()
        {
            var email = HttpContext.GetEmail();
            return await IsEnabled(email);
        }

        [Authorization]
        [HttpPost("Enable")]
        public async Task<IActionResult> Enable()
        {
            var email = HttpContext.GetEmail();
            var appUser = await userManager.FindByEmailAsync(email);

            var result = await userManager.SetTwoFactorEnabledAsync(appUser, true);

            return result.Succeeded ? Ok() : (StatusCodeResult)BadRequest();
        }

        [Authorization]
        [HttpPost("Disable")]
        public async Task<IActionResult> Disable()
        {
            var email = HttpContext.GetEmail();
            var appUser = await userManager.FindByEmailAsync(email);

            var result = await userManager.SetTwoFactorEnabledAsync(appUser, false);

            return result.Succeeded ? Ok() : (StatusCodeResult)BadRequest();
        }
    }
}