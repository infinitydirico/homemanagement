using HomeManagement.Models;
using MatBlazor;
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

        public async Task UploadFiles(IMatFileEntry[] files, string username)
        {
            using (var httpClient = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                var endpoint = configuration.GetSection("Endpoints").GetValue<string>("Storage");
                httpClient.BaseAddress = new Uri($"{endpoint}/api/");
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                httpClient.DefaultRequestHeaders.Add("Username", username);
                httpClient.DefaultRequestHeaders.Add("AppName", "HomeManagementWebApp");
                httpClient.DefaultRequestHeaders.Add("Path", "transactions");

                List<System.IO.Stream> streams = new List<System.IO.Stream>();

                for (int i = 0; i < files.Length; i++)
                {
                    var file = files[i];
                    var stream = file.Data;
                    content.Add(new StreamContent(stream), file.Name);
                }

                var response = await httpClient.PutAsync($"storage/send", content);

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
