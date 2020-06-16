using HomeManagement.Api.Core;
using HomeManagement.Api.Identity.SecurityCodes;
using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ICryptography cryptography;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthenticationController> logger;
        private readonly ICodesServices codesServices;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICryptography cryptography,
            IConfiguration configuration,
            ILogger<AuthenticationController> logger,
            ICodesServices codesServices)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cryptography = cryptography;
            this.configuration = configuration;
            this.logger = logger;
            this.codesServices = codesServices;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> Post([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogInformation($"Invalid model state: {string.Concat(ModelState.Values.Select(x => x.Errors.Select(r => r.ErrorMessage)))}");
                return BadRequest(ModelState);
            }

            var password = cryptography.Decrypt(userModel.Password);

            var user = await userManager.FindByEmailAsync(userModel.Email);            

            if (user == null) return BadRequest("Invalid email or password.");

            if (await userManager.IsLockedOutAsync(user)) return BadRequest($"The user {user.Email} has been locked out.");

            if (!await userManager.CheckPasswordAsync(user, password))
            {
                await userManager.AccessFailedAsync(user);
                return BadRequest("Invalid email or password.");
            }

            var twoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);

            if (twoFactorEnabled)
            {
                var userCode = codesServices.GetUserCode(userModel.Email);

                if (userCode.Code.Equals(default)) return NotFound("Code not found.");

                if (userCode.Code != userModel.SecurityCode) return BadRequest("Invalid code.");
            }            

            var result = await signInManager.PasswordSignInAsync(userModel.Email, password, true, false);
            
            if (!result.Succeeded) return BadRequest("Invalid email or password.");

            var roles = await userManager.GetRolesAsync(user);

            var token = roles.Any() ?
                TokenFactory.CreateToken(user.Email, roles, configuration["Issuer"], configuration["Audience"], configuration["SigningKey"], DateTime.UtcNow.AddDays(1)) :
                TokenFactory.CreateToken(user.Email, configuration["Issuer"], configuration["Audience"], configuration["SigningKey"]);

            var tokenResult = await userManager.SetAuthenticationTokenAsync(user, nameof(JwtSecurityToken), nameof(JwtSecurityToken), token);

            return Ok(new UserModel
            {
                Email = userModel.Email,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddDays(1)
            });
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(userModel.Email);

            var result = await userManager.RemoveAuthenticationTokenAsync(user, nameof(JwtSecurityToken), nameof(JwtSecurityToken));

            return Ok();
        }
    }
}