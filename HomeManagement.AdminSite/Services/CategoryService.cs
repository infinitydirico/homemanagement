using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Services
{
    public class CategoryService : IApiService, ICategoryService
    {
        private IMemoryCache _cache;
        private IHttpContextAccessor httpContextAccessor;

        public CategoryService(IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _cache = memoryCache;
            this.httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public string GetApiEndpoint() => this.GetEndpoint();

        public async Task<IEnumerable<UserCategoryModel>> GetUserCategories()
        {
            using (var client = this.CreateClient())
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var userModel = _cache.Get<UserModel>(ip);

                client.DefaultRequestHeaders.Add("Authorization", userModel.Token);

                var response = await client.GetAsync("Category/GetUsersCategories");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IEnumerable<UserCategoryModel>>(result);

                return data;
            }
        }

        public Task RemoveCategory(int userId, int categoryId)
        {
            throw new NotImplementedException();
        }
    }

    public interface ICategoryService
    {
        Task<IEnumerable<UserCategoryModel>> GetUserCategories();

        Task RemoveCategory(int userId, int categoryId);
    }
}
