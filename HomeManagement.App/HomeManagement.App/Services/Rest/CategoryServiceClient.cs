using HomeManagement.App.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest
{
    public class CategoryServiceClient
    {
        BaseRestClient restClient;

        public CategoryServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.BASEURL);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var api = Endpoints.Category.CATEGORY;
            var result = await restClient.Get<IEnumerable<Category>>(api);
            return result;
        }
    }
}
