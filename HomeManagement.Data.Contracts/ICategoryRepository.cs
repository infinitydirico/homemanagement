using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System.Collections.Generic;

namespace HomeManagement.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetUserCategories(string username);

        IEnumerable<Category> GetActiveUserCategories(string username);
    }
}
