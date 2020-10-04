using HomeManagement.API.Data.Querys.Account.Metrics;
using HomeManagement.API.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Accounts
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Route("api/account/v1")]
    [ApiController]
    public class AccountMetricController : HomeManagementController
    {
        private readonly IAccountAverageSeriesQuery accountAverageSeriesQuery;

        public AccountMetricController(IAccountAverageSeriesQuery accountAverageSeriesQuery)
        {
            this.accountAverageSeriesQuery = accountAverageSeriesQuery;
        }

        [HttpGet("avgseries")]
        public IActionResult AverageSeries()
        {            
            var result = accountAverageSeriesQuery.AccountsAvgSeries(Principal.Email);

            return Ok(result);
        }
    }
}
