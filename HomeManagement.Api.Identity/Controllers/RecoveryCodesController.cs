using HomeManagement.Api.Core.Extensions;
using HomeManagement.Api.Identity.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveryCodesController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public RecoveryCodesController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorization]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var email = HttpContext.GetEmail();
            var appUser = await userManager.FindByEmailAsync(email);

            var codes = await userManager.CountRecoveryCodesAsync(appUser);

            if (codes > 0) return BadRequest("Codes were already generated.");

            var result = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(appUser, 5);

            return Ok(result);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(string codes)
        {
            var result = await signInManager.TwoFactorRecoveryCodeSignInAsync(codes);

            if (result.IsLockedOut) return BadRequest("User is locked out.");

            if (result.IsNotAllowed) return BadRequest("This user is not allwoed for recovery codes sign in.");

            if (result.RequiresTwoFactor) return BadRequest("The user requires 2 factor authentication.");

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            await userManager.RedeemTwoFactorRecoveryCodeAsync(user, codes);
            
            return Ok();
        }
    }
}