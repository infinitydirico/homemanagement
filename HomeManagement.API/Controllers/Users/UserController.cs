using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using HomeManagement.Core.Extensions;
using HomeManagement.Models;
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

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorization]
        [HttpGet("downloaduserdata")]
        public IActionResult DownloadUserData()
        {
            var file = userService.DownloadUserData();

            var contentType = "application/octet-stream";

            return File(file.GetBytes(), contentType, "userdata.zip");
        }

        [Authorization]
        [AdminAuthorization]
        [HttpGet("getusers")]
        public IActionResult GetUsers()
        {
            return Ok(userService.GetUsers());
        }

        [HttpPost("CreateDefaultData")]
        public IActionResult CreateDefaultData([FromBody] UserModel userModel)
        {
            userService.CreateDefaultData(userModel);
            return Ok();
        }

        [Authorization]
        [AdminAuthorization]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            userService.DeleteUser(id);
            return Ok();
        }
    }
}