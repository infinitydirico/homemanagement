using HomeManagement.Api.Core.Email;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Controllers
{    
    [EnableCors("IdentityApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordManagementController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmailService emailService;

        public PasswordManagementController(UserManager<IdentityUser> userManager,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }

        [HttpPost]
        [Authorization]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Select(x => x.Value).ToList());

            var appUser = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var result = await userManager.ChangePasswordAsync(appUser, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded) return Ok();
            else return BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Forgot([FromBody] UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email)) return BadRequest("Email is required");

            var appUser = await userManager.FindByEmailAsync(userModel.Email);

            var token = await userManager.GeneratePasswordResetTokenAsync(appUser);

            var randomPassword = new Random().Next(1111, 9999).ToString();

            var resetResult = await userManager.ResetPasswordAsync(appUser, token, randomPassword);

            if (!resetResult.Succeeded) return BadRequest(resetResult.Errors.ToList());

            await emailService.Send(
                "no-reply@homemanagement.com",
                new List<string> { userModel.Email },
                "Temporary Password",
                $@"Here yours temporary password {randomPassword}. \r\n Update it as soon as you sign in the web page.",
                $@"<strong>Here yours temporary password {randomPassword}. \r\n Update it as soon as you sign in the web page.</strong>");

            return Ok();
        }
    }
}
