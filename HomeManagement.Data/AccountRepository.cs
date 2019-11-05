using HomeManagement.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Data
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Account entity) => GetById(entity.Id) != null;

        public IEnumerable<Account> GetAllByUser(string username)
        {
            var accountSet = platformContext.GetDbContext().Set<Account>().AsQueryable();
            var userSet = platformContext.GetDbContext().Set<User>().AsQueryable();

            var accounts = from account in accountSet
                           join user in userSet
                           on account.UserId equals user.Id
                           where user.Email.Equals(username)
                           select account;

            return accounts.ToList();
        }

        public override Account GetById(int id)
        {
            return platformContext.GetDbContext().Set<Account>().FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}
