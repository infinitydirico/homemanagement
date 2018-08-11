using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override Category GetById(int id) => platformContext.GetDbContext().Set<Category>().FirstOrDefault(x => x.Id.Equals(id));
    }
}
