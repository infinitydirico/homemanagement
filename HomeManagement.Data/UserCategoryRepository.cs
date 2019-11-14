using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HomeManagement.Data
{
    public class UserCategoryRepository : BaseRepository<UserCategory>, IUserCategoryRepository
    {
        public UserCategoryRepository(DbContext context)
            : base(context)
        {

        }
        public override bool Exists(UserCategory entity) => FirstOrDefault(x => x.UserId.Equals(entity.UserId) && x.CategoryId.Equals(entity.CategoryId)) != null;

        public override UserCategory GetById(int id) => throw new NotImplementedException();

        public bool UserHasAssociatedCategory(int userId, int categoryId)
        {
            var ucSet = context.Set<UserCategory>().AsQueryable();

            var userHasAssociatedCategory = ucSet.Any(x => x.UserId.Equals(userId) && x.CategoryId.Equals(categoryId));

            return userHasAssociatedCategory;
        }
    }
}
