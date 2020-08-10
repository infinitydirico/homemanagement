using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Services
{
    public class UserService : IApiService, IUserService
    {
        private IMemoryCache _cache;
        private IHttpContextAccessor httpContextAccessor;

        public UserService(IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _cache = memoryCache;
            this.httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public string GetApiEndpoint() => this.GetEndpoint();

        public async Task<IEnumerable<UserIdentityModel>> GetUsers()
        {
            using (var client = this.CreateIdentityClient())
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var userModel = _cache.Get<UserModel>(ip);

                client.DefaultRequestHeaders.Add("Authorization", userModel.Token);

                var response = await client.GetAsync("api/users");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IEnumerable<UserIdentityModel>>(result);

                return data;
            }
        }

        public async Task Delete(string email)
        {
            using (var client = this.CreateIdentityClient())
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var userModel = _cache.Get<UserModel>(ip);

                client.DefaultRequestHeaders.Add("Authorization", userModel.Token);

                var response = await client.DeleteAsync($"api/users/{email}");
                response.EnsureSuccessStatusCode();
            }
        }
    }

    public interface IUserService
    {
        Task<IEnumerable<UserIdentityModel>> GetUsers();

        Task Delete(string email);
    }
}
