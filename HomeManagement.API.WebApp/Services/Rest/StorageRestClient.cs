using HomeManagement.Models;
using Microsoft.AspNetCore.ProtectedBrowserStorage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeManagement.API.WebApp.Services.Rest
{
    public class StorageRestClient
    {
        private readonly ProtectedSessionStorage protectedSessionStorage;
        private readonly IConfiguration configuration;

        public StorageRestClient(ProtectedSessionStorage protectedSessionStorage, IConfiguration configuration)
        {
            this.protectedSessionStorage = protectedSessionStorage;
            this.configuration = configuration;
        }

        public async Task<List<StorageFileModel>> GetStorageFileModels()
        {
            using (var httpClient = new HttpClient())
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                var endpoint = configuration.GetSection("Endpoints").GetValue<string>("Storage");
                httpClient.BaseAddress = new Uri($"{endpoint}/api/");
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

                var response = await httpClient.GetAsync("storage");

                var content = await response.Content.ReadAsStringAsync();

                var result = Json.Deserialize<List<StorageFileModel>>(content);

                return result;
            }
        }

        public async Task<byte[]> DownloadFile(string tag)
        {
            using (var httpClient = new HttpClient())
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                var endpoint = configuration.GetSection("Endpoints").GetValue<string>("Storage");
                httpClient.BaseAddress = new Uri($"{endpoint}/api/");
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

                var response = await httpClient.GetAsync($"storage/{tag}");

                var content = await response.Content.ReadAsByteArrayAsync();

                return content;
            }
        }
    }
}
