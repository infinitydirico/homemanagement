using HomeManagement.API.Filters;
using HomeManagement.Business.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Transactions
{
    [ApiController]
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Transactions/v1")]
    public class TransactionsExtendedV1Controller : Controller
    {
        private readonly IAccountService accountService;
        private readonly ITransactionService transactionService;

        public TransactionsExtendedV1Controller(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet("account/{accountId}/page")]
        public IActionResult Page(int accountId, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            if (currentPage == default || pageSize == default) return BadRequest($"Current Page and Page Size cannot be less than 1.");

            var model = transactionService.Page(new TransactionPageModel
            {
                CurrentPage = currentPage,
                PageCount = pageSize,
                AccountId = accountId
            });

            return Ok(model.Transactions);
        }
    }
}
