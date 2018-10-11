using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Mapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public UserController(IUserRepository userRepository, IUserMapper userMapper)
        {
            this.userRepository = userRepository;
            this.userMapper = userMapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var emailClaim = HttpContext.GetEmailClaim();

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(emailClaim.Value));

            return Ok(userMapper.ToModel(user));
        }
    }
}