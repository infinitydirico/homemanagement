using System.Linq;
using System.Threading.Tasks;
using HomeManagement.Api.Core;
using HomeManagement.Api.Identity.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeManagement.Api.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization(Constants.Roles.Admininistrator)]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<RolesController> logger;

        public RolesController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RolesController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var user = await userManager.FindByNameAsync(email);

            if (user == null) return NotFound($"User: {email} does not exists.");

            var userRoles = await userManager.GetRolesAsync(user);

            return Ok(userRoles);
        }

        [HttpGet("available")]
        public IActionResult Available()
        {           
            return Ok(roleManager.Roles.Select(x => x.Name).ToList());
        }
    }
}
