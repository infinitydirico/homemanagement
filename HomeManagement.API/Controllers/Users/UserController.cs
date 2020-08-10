using HomeManagement.API.Filters;
using HomeManagement.Business.Contracts;
using HomeManagement.Core.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Users
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IUserSessionService userSessionService;

        public UserController(IUserService userService,
            IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.userSessionService = userSessionService;
        }

        [Authorization]
        [HttpGet("downloaduserdata")]
        public IActionResult DownloadUserData()
        {
            var user = userSessionService.GetAuthenticatedUser();

            var file = userService.DownloadUserData(user.Id);

            var contentType = "application/octet-stream";

            return File(file.GetBytes(), contentType, "userdata.zip");
        }
    }
}