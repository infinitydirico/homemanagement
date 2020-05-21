using HomeManagement.Api.Core.Email;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly ICryptography cryptography;

        public PasswordManagementController(UserManager<IdentityUser> userManager,
            IEmailService emailService,
            ICryptography cryptography)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.cryptography = cryptography;
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

            await emailService.Send(
                "no-reply@homemanagement.com",
                new List<string> { email },
                "Temporary Password",
                $@"Here yours temporary password {GetEnvironmentUrl() + token}. \r\n Update it as soon as you sign in the web page.",
                $@"<strong>Here yours temporary password {GetEnvironmentUrl() + token}. \r\n Update it as soon as you sign in the web page.</strong>");

            //var randomPassword = CreateRandomPassword();
            //var resetResult = await userManager.ResetPasswordAsync(appUser, token, randomPassword);

            //if (!resetResult.Succeeded) return BadRequest(resetResult.Errors.ToList());

            //await emailService.Send(
            //    "no-reply@homemanagement.com",
            //    new List<string> { email },
            //    "Temporary Password",
            //    $@"Here yours temporary password {randomPassword}. \r\n Update it as soon as you sign in the web page.",
            //    $@"<strong>Here yours temporary password {randomPassword}. \r\n Update it as soon as you sign in the web page.</strong>");

            return Ok();
        }

        private string GetEnvironmentUrl() => "http://localhost:5800/token?value=";

        private string CreateRandomPassword()
        {
            var random = new Random();

            var numericRandomPassword = random.Next(11111, 99999).ToString();
            var randomUppercaseLetter = random.Next(65, 90);
            var randomLowercaseLetter = random.Next(97, 122);
            var randomSpecialCharacter = random.Next(35, 38);

            var uppercaseLetter = char.ConvertFromUtf32(randomUppercaseLetter);
            var lowerCaseLetter = char.ConvertFromUtf32(randomLowercaseLetter);
            var specialCharacter = char.ConvertFromUtf32(randomSpecialCharacter);

            var randomPassword = numericRandomPassword + uppercaseLetter + lowerCaseLetter + specialCharacter;

            return randomPassword;
        }
    }
}
