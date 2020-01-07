using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace HomeManagement.AdminSite.Services
{
    public interface IApiService
    {
        IConfiguration Configuration { get; }

        string GetApiEndpoint();
    }

    public static class ApiServiceExtensions
    {
        public static HttpClient CreateClient(this IApiService apiService)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiService.GetEndpoint());
            return client;
        }

        public static HttpClient CreateIdentityClient(this IApiService apiService)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiService.GetIdentityEndpoint());
            return client;
        }

        public static string GetEndpoint(this IApiService apiService)
        {
            var endpoint = apiService.Configuration.GetSection("Endpoints").GetValue<string>("HomeManagement");
            return endpoint;
        }

        public static string GetIdentityEndpoint(this IApiService apiService)
        {
            var endpoint = apiService.Configuration.GetSection("Endpoints").GetValue<string>("Identity");
            return endpoint;
        }
    }
}
