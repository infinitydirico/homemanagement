using HomeManagement.Domain;
using System;
using System.Linq;

namespace HomeManagement.Data
{
    public class UserCategoryRepository : BaseRepository<UserCategory>, IUserCategoryRepository
    {
        public UserCategoryRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(UserCategory entity) => FirstOrDefault(x => x.UserId.Equals(entity.UserId) && x.CategoryId.Equals(entity.CategoryId)) != null;

        public override UserCategory GetById(int id) => throw new NotImplementedException();

        public bool UserHasAssociatedCategory(int userId, int categoryId)
        {
            var ucSet = platformContext.GetDbContext().Set<UserCategory>().AsQueryable();

            var userHasAssociatedCategory = ucSet.Any(x => x.UserId.Equals(userId) && x.CategoryId.Equals(categoryId));

            return userHasAssociatedCategory;
        }
    }
}
