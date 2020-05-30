using HomeManagement.Api.Core.Email;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HomeManagement.Api.Identity.Controllers
{
    [EnableCors("IdentityApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordManagementController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmailService emailService;
        private readonly ICryptography cryptography;
        private readonly IConfiguration configuration;

        public PasswordManagementController(UserManager<IdentityUser> userManager,
            IEmailService emailService,
            ICryptography cryptography,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.cryptography = cryptography;
            this.configuration = configuration;
        }

        [Authorization]
        [HttpPost("changepassword")]        
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Select(x => x.Value).ToList());

            var email = HttpContext.User.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;

            var appUser = await userManager.FindByEmailAsync(email);
            var result = await userManager.ChangePasswordAsync(appUser, 
                cryptography.Decrypt(model.CurrentPassword),
                cryptography.Decrypt(model.NewPassword));

            if (result.Succeeded) return Ok();
            else return BadRequest(result.Errors);
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> Forgot([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Select(e => e.Value).ToList());

            var email = model.Email;

            if (string.IsNullOrEmpty(email)) return BadRequest("Email is required");

            var appUser = await userManager.FindByEmailAsync(email);

            var token = await userManager.GeneratePasswordResetTokenAsync(appUser);
            var encodedToken = HttpUtility.UrlEncode(token);

            var url = GetEnvironmentUrl() + encodedToken + "&email=" + email;
            var href = "<a href=\"" + url + "\">Click Here</a>";

            await emailService.Send(
                "no-reply@homemanagement.com",
                new List<string> { email },
                "Password Recovery",
                $@"Click Here to change your password.",
                $@"<p>{href} to change your password.</p>");

            return Ok();
        }

        [HttpPost("tokenpasswordchange")]
        public async Task<IActionResult> TokenPasswordChange([FromBody] TokenPasswordModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Select(e => e.Value).ToList());

            var appUser = await userManager.FindByEmailAsync(model.Email);

            var result = await userManager.ResetPasswordAsync(appUser, model.Token, model.NewPassword);

            if (!result.Succeeded) return BadRequest();

            return Ok();
        }

        private string GetEnvironmentUrl()
        {
            var section = configuration.GetSection("Endpoints:WebApp");
            var tokenUrl = section.Value;
            tokenUrl += "/token?value=";
            return tokenUrl;
        }
    }
}
