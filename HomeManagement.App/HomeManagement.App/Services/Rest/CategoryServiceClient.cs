using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class CategoryServiceClient : ICategoryServiceClient
    {
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync(Constants.Endpoints.Category.CATEGORY)
                .ReadContent<IEnumerable<Category>>();
        }
    }

    public interface ICategoryServiceClient
    {
        Task<IEnumerable<Category>> GetCategories();
    }
}
