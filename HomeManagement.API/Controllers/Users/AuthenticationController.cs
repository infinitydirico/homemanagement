using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.API.Controllers.Users
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService userService;

        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserModel user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var signedInUser = await userService.SignIn(user);

            if (signedInUser == null) return BadRequest();

            return Ok(signedInUser);
        }

        [Authorization]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] UserModel user)
        {
            await userService.SignOut(user);
            return Ok();
        }
    }
}