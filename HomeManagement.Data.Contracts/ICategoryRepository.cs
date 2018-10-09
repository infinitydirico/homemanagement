using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category Add(Category entity, User user);

        void Remove(int categoryId, User user);
    }
}
