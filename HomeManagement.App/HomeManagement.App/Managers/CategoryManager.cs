using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.Rest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public interface ICategoryManager
    {
        Task<IEnumerable<Category>> GetCategories();
    }

    public class CategoryManager : ICategoryManager
    {
        protected ICategoryServiceClient categoryServiceClient = App._container.Resolve<ICategoryServiceClient>();
        private List<Category> categories = new List<Category>();

        public async Task<IEnumerable<Category>> GetCategories()
        {
            if (!categories.Any())
            {
                categories.AddRange(await categoryServiceClient.GetCategories());
            }

            return categories;
        }
    }
}
