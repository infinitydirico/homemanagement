using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IPlatformContext platformContext) : base(platformContext, new TransactionalRepository<Account>(platformContext))
        {
        }

        public override bool Exists(Account entity) => GetById(entity.Id) != null;

        public override Account GetById(int id)
        {
            using(var dbContext = platformContext.GetDbContext())
            {
                return dbContext.Set<Account>().FirstOrDefault(x => x.Id.Equals(id));
            }
        }
    }
}
