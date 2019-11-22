using HomeManagement.API.Filters;
using HomeManagement.Business.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Accounts
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountExtendedController : Controller
    {
        private readonly IAccountService accountService;

        public AccountExtendedController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("paging")]
        public IActionResult Page([FromBody]AccountPageModel model)
        {
            if (model == null) return BadRequest();

            model = accountService.Page(model);

            return Ok(model);
        }
    }
}