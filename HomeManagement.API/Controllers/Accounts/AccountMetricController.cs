using HomeManagement.API.Data.Querys.Account.Metrics;
using HomeManagement.API.Filters;
using HomeManagement.API.Infraestructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Accounts
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Route("api/account/v1")]
    [ApiController]
    public class AccountMetricController : ControllerBase
    {
        private readonly IAccountAverageSeriesQuery accountAverageSeriesQuery;

        public AccountMetricController(IAccountAverageSeriesQuery accountAverageSeriesQuery)
        {
            this.accountAverageSeriesQuery = accountAverageSeriesQuery;
        }

        [HttpGet("avgseries")]
        public IActionResult AverageSeries()
        {            
            var result = accountAverageSeriesQuery.AccountsAvgSeries(HttpContext.User.GetUserEmail());

            return Ok(result);
        }
    }
}
