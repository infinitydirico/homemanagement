using HomeManagement.API.Filters;
using HomeManagement.Business.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Controllers.Transactions
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Transactions")]
    public class TransactionsExtendedController : Controller
    {
        private readonly ITransactionService transactionService;

        public TransactionsExtendedController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpPost("paging")]
        public IActionResult Page([FromBody]TransactionPageModel model)
        {
            if (model == null) return BadRequest();

            model = transactionService.Page(model);

            return Ok(model);
        }

        [HttpGet("by/date/{year}/{month}")]
        public IActionResult ByDate(int year, int month)
        {
            if (year <= 0 || month <= 0) return BadRequest();

            var transactions = transactionService.FilterByDate(year, month);

            return Ok(transactions);
        }

        [HttpGet("by/date/{year}/{month}/account/{accountId}")]
        public IActionResult ByDateAndAccount(int year, int month, int accountId)
        {
            if (year <= 0 || month <= 0 || accountId <= 0) return BadRequest();

            var transactions = transactionService.FilterByDateAndAccount(year, month, accountId);

            return Ok(transactions);
        }

        [HttpGet("by/category/{category}")]
        public IActionResult ByCategory(int category)
        {
            if (category <= 0) return BadRequest();

            return Ok(transactionService.CategoryEvolution(category));
        }

        [HttpGet("by/account/{accountId}/category/{category}")]
        public IActionResult ByAccountAndCategory(int accountId, int category)
        {
            if (accountId <= 0 || category <= 0) return BadRequest();

            return Ok(transactionService.CategoryEvolutionByAccount(category, accountId));
        }

        [HttpGet("GetAutoComplete")]
        public IActionResult GetAutoComplete() => Ok(transactionService.GetTransactionsForAutoComplete());

        [HttpPost("updateAll")]
        public IActionResult UpdateAll([FromBody] IEnumerable<TransactionModel> models)
        {
            if (models == null || models.Count().Equals(default(int))) return BadRequest();

            foreach (var transaction in models)
            {
                transactionService.Update(transaction);
            }

            return Ok();
        }

        [HttpDelete("deleteall/{accountId}")]
        public IActionResult DeleteAll(int accountId)
        {
            if (accountId < 1) return BadRequest();

            transactionService.BatchDelete(accountId);

            return Ok();
        }
    }
}