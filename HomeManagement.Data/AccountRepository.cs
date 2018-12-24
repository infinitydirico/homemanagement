using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Account entity) => GetById(entity.Id) != null;

        public override Account GetById(int id)
        {
            return platformContext.GetDbContext().Set<Account>().FirstOrDefault(x => x.Id.Equals(id));
            //using(var dbContext = platformContext.GetDbContext())
            //{
            //    return dbContext.Set<Account>().FirstOrDefault(x => x.Id.Equals(id));
            //}
        }
    }
}
