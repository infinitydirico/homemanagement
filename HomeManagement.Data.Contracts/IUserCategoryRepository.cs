using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public interface IUserCategoryRepository : IRepository<UserCategory>
    {
        bool UserHasAssociatedCategory(int userId, int categoryId);
    }
}
