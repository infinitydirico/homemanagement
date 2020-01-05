using HomeManagement.API.Filters;
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
        public async Task<IActionResult> Get() => Ok(await healthCheckService.CheckHealthAsync());
    }
}