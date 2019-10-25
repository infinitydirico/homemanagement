using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Services
{
    public class CurrencyService : IApiService, ICurrencyService
    {
        private IMemoryCache _cache;
        private IHttpContextAccessor httpContextAccessor;

        public CurrencyService(IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _cache = memoryCache;
            this.httpContextAccessor = httpContextAccessor;
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public string GetApiEndpoint() => this.GetEndpoint();

        public async Task<IEnumerable<CurrencyModel>> GetCurrencies()
        {
            using (var client = this.CreateClient())
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var userModel = _cache.Get<UserModel>(ip);

                client.DefaultRequestHeaders.Add("Authorization", userModel.Token);

                var response = await client.GetAsync("currency");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IEnumerable<CurrencyModel>>(result);

                return data;
            }
        }
    }

    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyModel>> GetCurrencies();
    }
}
