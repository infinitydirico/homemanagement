using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeManagement.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context)
            : base(context)
        {

        }
        public override bool Exists(User entity) => GetById(entity.Id) != null;

        public User GetByEmail(string email) => FirstOrDefault(x => x.Email.Equals(email));

        public override User GetById(int id) => context.Set<User>().FirstOrDefault(x => x.Id.Equals(id));
    }
}
