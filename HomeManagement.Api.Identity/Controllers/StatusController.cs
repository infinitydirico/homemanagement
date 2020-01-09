using HomeManagement.Api.Core.HealthChecks;
using HomeManagement.Api.Identity.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.IO.File;

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

        [HttpGet("GetLogs")]
        public IActionResult GetLogs()
        {
            var bytes = ReadAllBytes($@"{Directory.GetCurrentDirectory()}\logs\logfile-{DateTime.Now.ToString("yyyyMMdd")}.txt");

            return File(bytes, "application/text");
        }
    }
}