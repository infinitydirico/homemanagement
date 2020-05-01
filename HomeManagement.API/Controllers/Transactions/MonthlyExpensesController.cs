using HomeManagement.API.Filters;
using HomeManagement.Business.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Transactions
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/MonthlyExpenses")]
    public class MonthlyExpensesController : Controller
    {
        private readonly IMonthlyExpenseService MonthlyExpenseService;

        public MonthlyExpensesController(IMonthlyExpenseService MonthlyExpenseService)
        {
            this.MonthlyExpenseService = MonthlyExpenseService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = MonthlyExpenseService.GetMonthlyExpenses();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] MonthlyExpenseModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MonthlyExpenseService.Save(model);

                if (result.IsSuccess) return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = MonthlyExpenseService.Remove(id);
            if (result.IsSuccess) return Ok();
            else return BadRequest();
        }
    }
}