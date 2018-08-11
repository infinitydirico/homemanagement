using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override Account GetById(int id)
        {
            using(var dbContext = platformContext.GetDbContext())
            {
                return dbContext.Set<Account>().FirstOrDefault(x => x.Id.Equals(id));
            }
        }
    }
}
