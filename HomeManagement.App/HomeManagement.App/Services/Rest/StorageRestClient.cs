using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Managers;
using HomeManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomeManagement.App.Services.Rest
{
    public class StorageRestClient
    {
        private const string AuthorizationHeader = "Authorization";

        public async Task<IEnumerable<StorageFileModel>> Get()
        {
            if (Connectivity.NetworkAccess.Equals(NetworkAccess.None)) throw new AppException($"No internet connection detected.");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.Endpoints.STORAGE_API);
                client.DefaultRequestHeaders.Add(AuthorizationHeader, GetToken());

                var result = await client.GetAsync("storage");

                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();

                var files = JsonConvert.DeserializeObject<IEnumerable<StorageFileModel>>(content);

                return files;
            }
        }

        private static string GetToken() => App._container.Resolve<IAuthenticationManager>().GetAuthenticatedUser().Token;
    }
}