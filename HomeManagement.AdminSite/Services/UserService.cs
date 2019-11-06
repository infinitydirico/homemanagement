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

        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            using (var client = this.CreateClient())
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var userModel = _cache.Get<UserModel>(ip);

                client.DefaultRequestHeaders.Add("Authorization", userModel.Token);

                var response = await client.GetAsync("user/getusers");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(result);

                return data;
            }
        }

        public async Task Delete(int userId)
        {
            using (var client = this.CreateClient())
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var userModel = _cache.Get<UserModel>(ip);

                client.DefaultRequestHeaders.Add("Authorization", userModel.Token);

                var response = await client.DeleteAsync($"user/{userId}");
                response.EnsureSuccessStatusCode();
            }
        }
    }

    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsers();

        Task Delete(int userId);
    }
}
