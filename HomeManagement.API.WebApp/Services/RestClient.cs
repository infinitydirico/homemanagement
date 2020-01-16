using Microsoft.AspNetCore.ProtectedBrowserStorage;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeManagement.API.WebApp.Services
{
    public class RestClient
    {
        private readonly ProtectedSessionStorage protectedSessionStorage;
        private readonly IConfiguration configuration;

        public RestClient(ProtectedSessionStorage protectedSessionStorage, IConfiguration configuration)
        {
            this.protectedSessionStorage = protectedSessionStorage;
            this.configuration = configuration;
        }

        #region Get

        public async Task<TResult> GetAsync<TResult>(string api, bool enableCache = true)
        {
            if (enableCache) return await GetAsyncWithCaching<TResult>(api);
            else return await InternalGetAsync<TResult>(api);
        }

        private async Task<TResult> GetAsyncWithCaching<TResult>(string api)
        {
            var cachedValue = await protectedSessionStorage.GetAsync<TResult>(api);
            if (cachedValue != null) return cachedValue;

            var result = await InternalGetAsync<TResult>(api);

            await protectedSessionStorage.SetAsync(api, result);

            return result;
        }

        private async Task<TResult> InternalGetAsync<TResult>(string api)
        {
            using (var httpClient = new HttpClient())
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                var endpoint = configuration.GetSection("Endpoints").GetValue<string>("HomeManagement");
                httpClient.BaseAddress = new Uri($"{endpoint}/api/");
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

                var response = await httpClient.GetAsync(api);

                var content = await response.Content.ReadAsStringAsync();

                var result = Json.Deserialize<TResult>(content);

                return result;
            }
        }

        #endregion

        #region Post

        public async Task PostAsync<TModel>(string api, TModel model)
        {
            using (var httpClient = new HttpClient())
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                var endpoint = configuration.GetSection("Endpoints").GetValue<string>("HomeManagement");
                httpClient.BaseAddress = new Uri($"{endpoint}/api/");
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

                var contentModel = Json.CreateJsonContent(model);

                var response = await httpClient.PostAsync(api, contentModel);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<TResult> PostAsync<TResult, TModel>(string api, TModel model, bool cacheEnabled = true)
        {
            if (cacheEnabled) return await PostAsyncWithCaching<TResult, TModel>(api, model);
            else return await InternalPostAsync<TResult, TModel>(api, model);
        }

        private async Task<TResult> PostAsyncWithCaching<TResult, TModel>(string api, TModel model)
        {
            var cachedValue = await protectedSessionStorage.GetAsync<TResult>(api);
            if (cachedValue != null) return cachedValue;

            var result = await InternalPostAsync<TResult, TModel>(api, model);

            await protectedSessionStorage.SetAsync(api, result);

            return result;
        }

        private async Task<TResult> InternalPostAsync<TResult, TModel>(string api, TModel model)
        {
            using (var httpClient = new HttpClient())
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                var endpoint = configuration.GetSection("Endpoints").GetValue<string>("HomeManagement");
                httpClient.BaseAddress = new Uri($"{endpoint}/api/");
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

                var contentModel = Json.CreateJsonContent(model);

                var response = await httpClient.PostAsync(api, contentModel);

                var content = await response.Content.ReadAsStringAsync();

                var result = Json.Deserialize<TResult>(content);

                return result;
            }
        }

        #endregion

        #region Put

        public async Task PutAsync<TModel>(string api, TModel model)
        {
            using (var httpClient = new HttpClient())
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                var endpoint = configuration.GetSection("Endpoints").GetValue<string>("HomeManagement");
                httpClient.BaseAddress = new Uri($"{endpoint}/api/");
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

                var contentModel = Json.CreateJsonContent(model);

                var response = await httpClient.PutAsync(api, contentModel);
                response.EnsureSuccessStatusCode();
            }
        }

        #endregion

        #region Delete

        public async Task DeleteAsync(string api, int id)
        {
            using (var httpClient = new HttpClient())
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                var endpoint = configuration.GetSection("Endpoints").GetValue<string>("HomeManagement");
                httpClient.BaseAddress = new Uri($"{endpoint}/api/");
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

                var response = await httpClient.DeleteAsync($"{api}/{id}");

                response.EnsureSuccessStatusCode();
            }
        }

        #endregion
    }
}
