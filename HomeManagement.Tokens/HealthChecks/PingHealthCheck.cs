using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace HomeManagement.Api.Core.HealthChecks
{
    public class PingHealthCheck : IHealthCheck
    {
        private string _host;
        private int _timeout;

        public PingHealthCheck(string host, int timeout)
        {
            _host = host;
            _timeout = timeout;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(_host, _timeout);
                    if (reply.Status != IPStatus.Success)
                    {
                        return HealthCheckResult.Unhealthy();
                    }

                    if (reply.RoundtripTime >= _timeout)
                    {
                        return HealthCheckResult.Degraded();
                    }

                    return HealthCheckResult.Healthy();
                }
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
