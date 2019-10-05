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
    public class AccountStatisticsController : Controller
    {
        private readonly IMetricsService metricsService;

        public AccountStatisticsController(IMetricsService metricsService)
        {
            this.metricsService = metricsService;
        }

        [HttpGet("{id}/overall")]
        public IActionResult Overall(int id)
        {
            if (id < 0) return BadRequest();

            return Ok(metricsService.GetAccountOverview(id));
        }

        [HttpGet("getbalance")]
        public IActionResult GetBalance()
        {
            return Ok(metricsService.GetUserBalance());
        }

        [HttpGet("incomes")]
        public IActionResult Incomes()
        {
            return Ok(metricsService.GetIncomesMetric());
        }

        [HttpGet("outcomes")]
        public IActionResult Outcomes()
        {
            return Ok(metricsService.GetOutcomesMetric());
        }
    }
}