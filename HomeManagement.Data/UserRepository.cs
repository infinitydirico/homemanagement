using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(User entity) => GetById(entity.Id) != null;

        public User GetByEmail(string email) => FirstOrDefault(x => x.Email.Equals(email));

        public override User GetById(int id) => platformContext.GetDbContext().Set<User>().FirstOrDefault(x => x.Id.Equals(id));
    }
}
