using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace HomeManagement.API.Controllers.Transactions
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Transactions")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            if (claim == null) return BadRequest();

            return Ok(transactionService.GetAll());

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(transactionService.Get(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransactionModel model)
        {
            if (model == null) return BadRequest();

            transactionService.Add(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]TransactionModel model)
        {
            if (model == null) return BadRequest();

            transactionService.Update(model);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id < 1) return BadRequest();

            transactionService.Delete(id);

            return Ok();
        }
    }
}