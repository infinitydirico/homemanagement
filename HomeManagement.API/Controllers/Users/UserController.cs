using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using HomeManagement.Core.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Users
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("downloaduserdata")]
        public IActionResult DownloadUserData()
        {
            var file = userService.DownloadUserData();

            var contentType = "application/octet-stream";

            return File(file.GetBytes(), contentType, "userdata.zip");
        }

        [AdminAuthorization]
        [HttpGet("getusers")]
        public IActionResult GetUsers()
        {
            return Ok(userService.GetUsers());
        }

        [HttpPost]
        public IActionResult CreateDefaultData()
        {
            return Ok();
        }

        [AdminAuthorization]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            userService.DeleteUser(id);
            return Ok();
        }
    }
}