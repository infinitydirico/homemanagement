using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System.Collections.Generic;

namespace HomeManagement.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category Add(Category entity, User user);

        void Remove(int categoryId, User user);

        IEnumerable<Category> GetUserCategories(string username);

        IEnumerable<Category> GetActiveUserCategories(string username);
    }
}
