using HomeManagement.Api.Identity.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Controllers
{
    [Authorization]
    [EnableCors("IdentityApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordManagementController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public PasswordManagementController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Select(x => x.Value).ToList());

            var appUser = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var result = await userManager.ChangePasswordAsync(appUser, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded) return Ok();
            else return BadRequest(result.Errors);
        }
    }
}
