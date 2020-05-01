using HomeManagement.AdminSite.Services;
using HomeManagement.Api.Core.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.IO;
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

        public async Task<IActionResult> DownloadIdentityLogs()
        {
            var result = await endpointsHealthService.GetLogs("Identity");
            var stream = GenerateStreamFromString(result);
            return File(stream, "text/plain", "logs.txt");
        }

        public async Task<IActionResult> DownloadLogs()
        {
            var result = await endpointsHealthService.GetLogs("HomeManagement");
            var stream = GenerateStreamFromString(result);
            return File(stream, "text/plain", "logs.txt");
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}