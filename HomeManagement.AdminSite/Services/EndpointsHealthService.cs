using HomeManagement.Api.Core.HealthChecks;
using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Services
{
    public interface IEndpointsHealthService
    {
        Task<IEnumerable<HealthReportModel>> GetApiHealthReport();

        Task<IEnumerable<HealthReportModel>> GetIdentityHealthReport();

        Task<string> GetLogs(string serviceName);
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

        public async Task<IEnumerable<HealthReportModel>> GetApiHealthReport()
        {
            using (var client = this.CreateClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", GetAuthorizationHeader());

                var response = await client.GetAsync("api/status");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IEnumerable<HealthReportModel>>(result);

                return data;
            }
        }

        public async Task<IEnumerable<HealthReportModel>> GetIdentityHealthReport()
        {
            using (var client = this.CreateIdentityClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", GetAuthorizationHeader());

                var response = await client.GetAsync("api/status");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IEnumerable<HealthReportModel>>(result);

                return data;
            }
        }

        public string GetAuthorizationHeader()
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var userModel = memoryCache.Get<UserModel>(ip);

            return userModel.Token;
        }

        public async Task<string> GetLogs(string serviceName)
        {
            using (var client = serviceName.Equals("Identity") ? this.CreateIdentityClient() : this.CreateClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", GetAuthorizationHeader());

                var response = await client.GetAsync("api/status/getlogs");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
