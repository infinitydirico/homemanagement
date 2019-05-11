using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Services.Components;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public static class RestClientFactory
    {
        private const string AuthorizationHeader = "Authorization";

        public static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Constants.Endpoints.BASEURL);
            return client;
        }

        public static HttpClient CreateAuthenticatedClient()
        {
            var client = CreateClient();
            client.DefaultRequestHeaders.Add(AuthorizationHeader, GetToken());
            return client;
        }

        public static async Task<TValue> ReadContent<TValue>(this Task<HttpResponseMessage> responseMessage)
        {
            var response = (await responseMessage);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var objectResult = JsonConvert.DeserializeObject<TValue>(content);
            return objectResult;
        }

        private static string GetToken() => App._container.Resolve<IApplicationValues>().Get("header");
    }
}
