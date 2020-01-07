using HomeManagement.AdminSite.Services;
using HomeManagement.Api.Core.HealthChecks;
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
            var models = new List<HealthReportModel>();
            models.AddRange(await endpointsHealthService.GetIdentityHealthReport());
            models.AddRange(await endpointsHealthService.GetApiHealthReport());
            models.AddRange(HealthReportModel.CreateFromReport(await healthCheckService.CheckHealthAsync(), "Admin"));

            return View(models);
        }
    }
}