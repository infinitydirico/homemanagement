using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Data
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(DbContext context)
            :base(context)
        {

        }

        public override bool Exists(Account entity) => GetById(entity.Id) != null;

        public IEnumerable<Account> GetAllByUser(string username)
        {
            var accountSet = context.Set<Account>().AsQueryable();
            var userSet = context.Set<User>().AsQueryable();

            var accounts = from account in accountSet
                           join user in userSet
                           on account.UserId equals user.Id
                           where user.Email.Equals(username)
                           select account;

            return accounts.ToList();
        }

        public override Account GetById(int id)
        {
            return context.Set<Account>().FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}
