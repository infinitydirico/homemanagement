using HomeManagement.Api.Core;
using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Controllers
{
    [EnableCors("IdentityApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ICryptography cryptography;
        private readonly IConfiguration configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICryptography cryptography,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cryptography = cryptography;
            this.configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var password = cryptography.Decrypt(userModel.Password);

            var result = await signInManager.PasswordSignInAsync(userModel.Email, password, true, false);

            if (!result.Succeeded) return BadRequest();

            var user = await userManager.FindByEmailAsync(userModel.Email);

            var token = TokenFactory.CreateToken(user.Email, configuration["Issuer"], configuration["Audience"], configuration["SigningKey"]);

            var tokenResult = await userManager.SetAuthenticationTokenAsync(user, nameof(JwtSecurityToken), nameof(JwtSecurityToken), token);

            return Ok();
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