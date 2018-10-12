using HomeManagement.Domain;
using System;

namespace HomeManagement.Data
{
    public class UserCategoryRepository : BaseRepository<UserCategory>, IUserCategoryRepository
    {
        public UserCategoryRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(UserCategory entity) => FirstOrDefault(x => x.UserId.Equals(entity.UserId) && x.CategoryId.Equals(entity.CategoryId)) != null;

        public override UserCategory GetById(int id) => throw new NotImplementedException();
    }
}
