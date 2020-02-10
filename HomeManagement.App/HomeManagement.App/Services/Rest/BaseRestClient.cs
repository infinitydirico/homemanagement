using HomeManagement.App.Common;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomeManagement.App.Services.Rest
{
    internal class BaseRestClient
    {
        private const string AuthorizationHeader = "Authorization";
        string token;

        public BaseRestClient(string serverEndpoint)
        {
            Endpoint = new Uri(serverEndpoint);
        }

        public Uri Endpoint { get; }

        public async Task<TResult> Get<TResult>(string api)
        {
            CheckForInternetConnection();

            using (var client = await CreateAuthenticatedClient())
            {
                var result = await client.GetAsync(api);
                result.EnsureSuccessStatusCode();
                var content = await ReadJsonResponse<TResult>(result);
                return content;
            }
        }

        public async Task Post(string api, object obj)
        {
            CheckForInternetConnection();

            using (var client = await CreateAuthenticatedClient())
            {
                var result = await client.PostAsync(api, SerializeToJson(obj));
                result.EnsureSuccessStatusCode();
            }
        }

        public async Task<TResult> Post<TResult>(string api, object obj)
        {
            CheckForInternetConnection();

            using (var client = await CreateAuthenticatedClient())
            {
                var result = await client.PostAsync(api, SerializeToJson(obj));
                result.EnsureSuccessStatusCode();
                var content = await ReadJsonResponse<TResult>(result);
                return content;
            }
        }

        public async Task Put(string api, object obj)
        {
            CheckForInternetConnection();

            using (var client = await CreateAuthenticatedClient())
            {
                var result = await client.PutAsync(api, SerializeToJson(obj));
                result.EnsureSuccessStatusCode();
            }
        }

        public async Task<TResult> Put<TResult>(string api, object obj)
        {
            CheckForInternetConnection();

            using (var client = await CreateAuthenticatedClient())
            {
                var result = await client.PutAsync(api, SerializeToJson(obj));
                result.EnsureSuccessStatusCode();
                var content = await ReadJsonResponse<TResult>(result);
                return content;
            }
        }

        public async Task Delete(string api)
        {
            CheckForInternetConnection();

            using (var client = await CreateAuthenticatedClient())
            {
                var result = await client.DeleteAsync(api);
                result.EnsureSuccessStatusCode();
            }
        }

        private async Task<string> GetToken()
        {
            token = token ?? await SecureStorage.GetAsync("Token");
            return token;
        }

        public HttpClient CreateClient()
        {
            CheckForInternetConnection();

            var client = new HttpClient();
            client.BaseAddress = Endpoint;
            return client;
        }

        public async Task<HttpClient> CreateAuthenticatedClient()
        {
            var client = CreateClient();
            client.DefaultRequestHeaders.Add(AuthorizationHeader, await GetToken());
            return client;
        }

        public StringContent SerializeToJson(object data)
            => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        public async Task<TResult> ReadJsonResponse<TResult>(HttpResponseMessage message)
        {
            var content = await message.Content.ReadAsStringAsync();
            var objectResult = JsonConvert.DeserializeObject<TResult>(content);
            return objectResult;
        }

        private void CheckForInternetConnection()
        {
            if (Connectivity.NetworkAccess.Equals(NetworkAccess.None)) throw new AppException($"No internet connection detected.");
        }
    }
}
