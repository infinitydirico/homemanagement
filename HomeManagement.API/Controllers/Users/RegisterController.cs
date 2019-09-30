using HomeManagement.API.Business;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.API.Controllers.Users
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Register")]
    public class RegisterController : Controller
    {
        private readonly IUserService userService;

        public RegisterController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel user)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await userService.CreateUser(user);

            if (result.Result.Equals(Result.Succeed))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}