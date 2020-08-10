using HomeManagement.Api.Core.Extensions;
using HomeManagement.Api.Identity.Authentication;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Api.Identity.SecurityCodes;
using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthenticationController> logger;
        private readonly AuthenticationStrategy authenticationStrategy;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICryptography cryptography,
            IConfiguration configuration,
            ILogger<AuthenticationController> logger,
            ICodesServices codesServices)
        {
            this.userManager = userManager;
            this.logger = logger;
            authenticationStrategy = new AuthenticationStrategy(userManager, signInManager, cryptography, configuration, logger, codesServices);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> Post([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogInformation($"Invalid model state: {string.Concat(ModelState.Values.Select(x => x.Errors.Select(r => r.ErrorMessage)))}");
                return BadRequest(ModelState);
            }

            var authenticationResult = await authenticationStrategy.Authenticate(userModel, HttpContext.GetMobileHeader());

            if (authenticationResult.Succeed)
            {
                var authenticatedUser = await authenticationStrategy.GetAuthenticatedUser();
                return Ok(authenticatedUser);
            }
            else return BadRequest(authenticationResult.Error);
        }

        [MobileAuthorization]
        [HttpPost("MobileSignIn")]
        public async Task<IActionResult> MobileSignIn([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogInformation($"Invalid model state: {string.Concat(ModelState.Values.Select(x => x.Errors.Select(r => r.ErrorMessage)))}");
                return BadRequest(ModelState);
            }

            var authenticationResult = await authenticationStrategy.Authenticate(userModel);
            
            if (authenticationResult.Succeed)
            {
                var authenticatedUser = await authenticationStrategy.GetAuthenticatedUser();
                return Ok(authenticatedUser);
            }
            else return BadRequest(authenticationResult.Error);
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