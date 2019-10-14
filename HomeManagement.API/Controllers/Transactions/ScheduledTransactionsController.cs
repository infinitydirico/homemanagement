using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Transactions
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/ScheduledTransactions")]
    public class ScheduledTransactionsController : Controller
    {
        private readonly IScheduledTransactionService scheduledTransactionService;

        public ScheduledTransactionsController(IScheduledTransactionService scheduledTransactionService)
        {
            this.scheduledTransactionService = scheduledTransactionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = scheduledTransactionService.GetScheduledTransactions();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ScheduledTransactionModel model)
        {
            if (ModelState.IsValid)
            {
                var result = scheduledTransactionService.Save(model);

                if (result.IsSuccess) return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = scheduledTransactionService.Remove(id);
            if (result.IsSuccess) return Ok();
            else return BadRequest();
        }
    }
}