using HomeManagement.API.Business;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Mapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.API.Controllers.Users
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IUserMapper userMapper;
        private readonly IUserService userService;

        public UserController(IUserRepository userRepository, 
            IUserMapper userMapper,
            IUserService userService)
        {
            this.userRepository = userRepository;
            this.userMapper = userMapper;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var emailClaim = HttpContext.GetEmailClaim();

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(emailClaim.Value));

            return Ok(userMapper.ToModel(user));
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

        [AdminAuthorization]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await userService.DeleteUser(id);
            return Ok();
        }
    }
}