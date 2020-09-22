using HomeManagement.API.Data.Querys.Transaction;
using HomeManagement.API.Filters;
using HomeManagement.Mapper;
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
        private readonly ITransactionPagingQuery transactionPagingQuery;
        private readonly ITransactionMapper transactionMapper;

        public TransactionsExtendedV1Controller(ITransactionPagingQuery transactionPagingQuery,
            ITransactionMapper transactionMapper)
        {
            this.transactionPagingQuery = transactionPagingQuery;
            this.transactionMapper = transactionMapper;
        }

        [HttpGet("account/{accountId}/page")]
        public IActionResult Page(int accountId, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            if (accountId == default) return BadRequest($"Account id cannot be empty");
            if (currentPage == default || pageSize == default) return BadRequest($"Current Page and Page Size cannot be less than 1.");

            var entities = transactionPagingQuery.GetPage(accountId, currentPage, pageSize);

            return Ok(transactionMapper.ToModels(entities));
        }

        [HttpGet("account/{accountId}/filter/byName/{name}")]
        public IActionResult Filter(int accountId, string name, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            if (accountId == default) return BadRequest($"Account id cannot be empty");
            if (string.IsNullOrWhiteSpace(name)) return BadRequest($"name cannot be empty");

            if (currentPage == default || pageSize == default) return BadRequest($"Current Page and Page Size cannot be less than 1.");

            var entities = transactionPagingQuery.GetPage(accountId, name, currentPage, pageSize);

            return Ok(transactionMapper.ToModels(entities));
        }

        [HttpGet("account/{accountId}/filter/byCategory/{categoryId}")]
        public IActionResult Filter(int accountId, int categoryId, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            if (accountId == default) return BadRequest($"Account id cannot be empty");
            if (categoryId == default) return BadRequest($"Category id cannot be empty");

            if (currentPage == default || pageSize == default) return BadRequest($"Current Page and Page Size cannot be less than 1.");

            var entities = transactionPagingQuery.GetPage(accountId, categoryId, currentPage, pageSize);

            return Ok(transactionMapper.ToModels(entities));
        }
    }
}
