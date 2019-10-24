using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Services
{
    public class ConfigurationSettingsService : IConfigurationSettingsService, IApiService
    {
        private IMemoryCache _cache;
        private IHttpContextAccessor httpContextAccessor;

        public ConfigurationSettingsService(IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _cache = memoryCache;
            this.httpContextAccessor = httpContextAccessor;
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public async Task<IEnumerable<ConfigurationSettingModel>> GetSettings()
        {
            using (var client = this.CreateClient())
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var userModel = _cache.Get<UserModel>(ip);

                client.DefaultRequestHeaders.Add("Authorization", userModel.Token);

                var response = await client.GetAsync("configurationSettings");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IEnumerable<ConfigurationSettingModel>>(result);

                return data;
            }
        }

        public string GetApiEndpoint() => this.GetEndpoint();

        public async Task Update(ConfigurationSettingModel model)
        {
            using (var client = this.CreateClient())
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var userModel = _cache.Get<UserModel>(ip);

                client.DefaultRequestHeaders.Add("Authorization", userModel.Token);

                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("configurationSettings", content);
                response.EnsureSuccessStatusCode();
            }
        }
    }

    public interface IConfigurationSettingsService
    {
        Task<IEnumerable<ConfigurationSettingModel>> GetSettings();

        Task Update(ConfigurationSettingModel model);
    }
}
