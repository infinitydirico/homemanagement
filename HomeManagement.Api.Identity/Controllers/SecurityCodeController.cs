using HomeManagement.Api.Identity.Filters;
using HomeManagement.Api.Identity.SecurityCodes;
using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace HomeManagement.Api.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityCodeController : ControllerBase
    {
        private readonly ICodesServices codesServices;

        public SecurityCodeController(ICodesServices codesServices)
        {
            this.codesServices = codesServices;
        }

        [Authorization]
        [HttpGet]
        public IActionResult Get()
        {
            var email = HttpContext.User.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;
            var user = codesServices.GetUserCode(email);

            if (user.Code.Equals(default)) return NotFound();
            else return Ok(new UserCodeModel
            {
                Email = user.Email,
                Code = user.Code,
                Expiration = user.CodeExpirationStamp
            });
        }

        [HttpPost]
        public IActionResult Post(UserCodeModel userCodeModel)
        {
            var user = codesServices.GetUserCode(userCodeModel.Email);

            if (user.Code.Equals(default)) return NotFound();
            else
            {
                if (user.Code != userCodeModel.Code) return BadRequest("Wrong code.");

                return Ok();
            }
        }
    }
}