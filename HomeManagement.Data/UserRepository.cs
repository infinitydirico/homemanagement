using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override User GetById(int id)
        {
            using (var dbContext = platformContext.GetDbContext())
            {
                return dbContext.Set<User>().FirstOrDefault(x => x.Id.Equals(id));
            }
        }
    }
}
