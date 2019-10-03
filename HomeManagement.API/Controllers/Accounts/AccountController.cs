using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Accounts
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }       

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(accountService.GetAccounts());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = accountService.Get(id);

            if (account == null) return NotFound();

            return Ok(account);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AccountModel model)
        {
            if (model == null && !ModelState.IsValid) return BadRequest();

            accountService.Add(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]AccountModel model)
        {
            if (model == null && !ModelState.IsValid) return BadRequest();

            accountService.Update(model);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id < 1) return BadRequest();

            var result = accountService.Delete(id);

            if (result.Result.Equals(Result.Error)) return BadRequest(result.Errors);

            return Ok();
        }
    }
}