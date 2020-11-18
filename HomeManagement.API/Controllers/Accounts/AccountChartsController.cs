using HomeManagement.API.Business;
using HomeManagement.API.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Accounts
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountChartsController : Controller
    {
        private readonly IMetricsService metricsService;

        public AccountChartsController(IMetricsService metricsService)
        {
            this.metricsService = metricsService;
        }

        [HttpGet("accountsevolution")]
        [Cache(key: nameof(AccountChartsController.AccountsEvolution), slidingExpiration: 1)]
        public IActionResult AccountsEvolution()
        {
            var model = metricsService.GetAccountsBalancesEvolution();
            return Ok(model);
        }

        [HttpGet("{id}/accountevolution")]
        public IActionResult AccountEvolution(int id)
        {
            var model = metricsService.GetAccountEvolution(id);
            return Ok(model);
        }

        [HttpGet("toptransactions/{month}")]
        [Cache(key: nameof(AccountChartsController.AccountTopTransactions), slidingExpiration: 1)]
        public IActionResult AccountTopTransactions(int month)
        {
            var model = metricsService.TopTransactionsByMonth(month);
            return Ok(model);
        }

        [HttpGet("{id}/toptransactions/{month}")]
        public IActionResult AccountTopTransactions(int id, int month)
        {
            var result = metricsService.TopTransactionsByAccountAndMonth(id, month);

            return Ok(result);
        }
    }
}