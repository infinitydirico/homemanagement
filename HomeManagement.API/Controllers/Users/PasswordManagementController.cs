using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.API.Controllers.Users
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/PasswordManagement")]
    public class PasswordManagementController : Controller
    {
        private readonly IUserService userService;

        public PasswordManagementController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Select(x => x.Value).ToList());

            var result = await userService.ChangePassword(model.CurrentPassword, model.NewPassword);

            if (result.IsSuccess) return Ok();
            else return BadRequest(result.Errors);
        }
    }
}