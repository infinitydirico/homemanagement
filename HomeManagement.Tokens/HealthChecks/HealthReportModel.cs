using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Api.Core.HealthChecks
{
    public class HealthReportModel
    {
        public string ServiceName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public IEnumerable<string> Data { get; set; }

        public static IEnumerable<HealthReportModel> CreateFromReport(HealthReport healthReport, string serviceName)
        {
            return healthReport.Entries.Select(x => new HealthReportModel
            {
                ServiceName = serviceName,
                Name = x.Key,
                Description = x.Value.Description,
                Status = x.Value.Status.ToString(),
                Data = x.Value.Data.Select(d => d.Value.ToString())
            }).ToList();
        }
    }
}
