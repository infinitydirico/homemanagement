using HomeManagement.AdminSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    public class HealthController : Controller
    {
        private readonly HealthCheckService healthCheckService;
        private readonly IEndpointsHealthService endpointsHealthService;

        public HealthController(HealthCheckService healthCheckService, IEndpointsHealthService endpointsHealthService)
        {
            this.healthCheckService = healthCheckService;
            this.endpointsHealthService = endpointsHealthService;
        }

        public async Task<IActionResult> Index()
        {
            var identityReport = await endpointsHealthService.GetIdentityHealthReport();
            var apiReport = await endpointsHealthService.GetApiHealthReport();
            var report = await healthCheckService.CheckHealthAsync();

            return View(new List<HealthReport>
            {
                identityReport,
                apiReport,
                report
            });
        }
    }
}