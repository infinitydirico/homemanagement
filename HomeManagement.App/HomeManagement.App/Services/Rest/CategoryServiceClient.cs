using HomeManagement.App.Common;
using HomeManagement.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class CategoryServiceClient : BaseService, ICategoryServiceClient
    {
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await this.Get<IEnumerable<Category>>(Constants.Endpoints.Category.CATEGORY);
        }
    }

    public interface ICategoryServiceClient
    {
        Task<IEnumerable<Category>> GetCategories();
    }
}
