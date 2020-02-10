using HomeManagement.App.Common;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomeManagement.App.Services.Rest
{
    public static class RestClientFactory
    {
        private const string AuthorizationHeader = "Authorization";

        public static HttpClient CreateClient()
        {
            CheckForInternetConnection();

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
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();            
            var objectResult = JsonConvert.DeserializeObject<TValue>(content);
            return objectResult;
        }

        public static StringContent SerializeToJson(this object data)
            => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        private static string GetToken()
        {
            return SecureStorage.GetAsync("Token").GetAwaiter().GetResult();
        }

        private static void CheckForInternetConnection()
        {
            if (Connectivity.NetworkAccess.Equals(NetworkAccess.None)) throw new AppException($"No internet connection detected.");
        }
    }
}
