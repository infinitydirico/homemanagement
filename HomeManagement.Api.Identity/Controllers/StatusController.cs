using HomeManagement.Api.Core.HealthChecks;
using HomeManagement.Api.Identity.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Controllers
{
    [AdminAuthorization]
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly HealthCheckService healthCheckService;

        public StatusController(HealthCheckService healthCheckService)
        {
            this.healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var reports = await healthCheckService.CheckHealthAsync();
            var models = HealthReportModel.CreateFromReport(reports, "Identity");
            return Ok(models);
        }
    }
}