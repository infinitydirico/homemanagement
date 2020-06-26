using HomeManagement.Api.Core.Extensions;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Api.Identity.SecurityCodes;
using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [MobileAuthorization]
        [HttpGet]
        public IActionResult Get()
        {
            var email = HttpContext.GetEmail();
            var user = codesServices.GetUserCode(email);

            if (user == null || user.Code.Equals(default)) return NotFound();
            else return Ok(new UserCodeModel
            {
                Email = user.Email,
                Code = user.Code,
                Expiration = user.CodeExpirationStamp
            });
        }

        [MobileAuthorization]
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