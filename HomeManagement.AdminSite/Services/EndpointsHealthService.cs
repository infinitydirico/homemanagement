using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Services
{
    public interface IEndpointsHealthService
    {
        Task<HealthReport> GetApiHealthReport();

        Task<HealthReport> GetIdentityHealthReport();
    }

    public class EndpointsHealthService : IEndpointsHealthService, IApiService
    {
        private readonly IMemoryCache memoryCache;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EndpointsHealthService(IConfiguration configuration,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            this.memoryCache = memoryCache;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IConfiguration Configuration { get; }

        public string GetApiEndpoint() => this.GetEndpoint();

        public async Task<HealthReport> GetApiHealthReport()
        {
            using (var client = this.CreateClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", GetAuthorizationHeader());

                var response = await client.GetAsync("api/status");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<HealthReport>(result);

                return data;
            }
        }

        public async Task<HealthReport> GetIdentityHealthReport()
        {
            using (var client = this.CreateIdentityClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", GetAuthorizationHeader());

                var response = await client.GetAsync("api/status");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<HealthReport>(result);

                return data;
            }
        }

        public string GetAuthorizationHeader()
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var userModel = memoryCache.Get<UserModel>(ip);

            return userModel.Token;
        }
    }
}
