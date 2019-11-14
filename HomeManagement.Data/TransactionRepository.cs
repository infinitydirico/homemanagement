using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Data
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DbContext context)
            : base(context)
        {

        }
        public override bool Exists(Transaction entity) => GetById(entity.Id) != null;

        public IEnumerable<Transaction> GetByAccount(int accountId)
        {
            var transactionSet = context.Set<Transaction>().AsQueryable();

            var accountSet = context.Set<Account>().AsQueryable();

            return (from t in transactionSet
                    join a in accountSet
                    on t.AccountId equals a.Id
                    where a.Id.Equals(accountId)
                    select t).ToList();
        }

        public override Transaction GetById(int id) => context.Set<Transaction>().FirstOrDefault(x => x.Id.Equals(id));

        public IEnumerable<Transaction> GetByMeasurableAccount(int accountId)
        {
            var transactionSet = context.Set<Transaction>().AsQueryable();

            var accountSet = context.Set<Account>().AsQueryable();

            var transactions = (from t in transactionSet
                                join a in accountSet
                                on t.AccountId equals a.Id
                                where a.Measurable && a.Id.Equals(accountId)
                                select t).ToList();

            return transactions;
        }

        public IEnumerable<Transaction> GetByUser(string email)
        {
            var transactionSet = context.Set<Transaction>().AsQueryable();

            var accountSet = context.Set<Account>().AsQueryable();

            var userSet = context.Set<User>().AsQueryable();

            var transaction = from t in transactionSet
                              join a in accountSet
                              on t.AccountId equals a.Id
                              join u in userSet
                              on a.UserId equals u.Id
                              where u.Email.Equals(email)
                              select t;

            return transaction.ToList();
        }

        public void DeleteAllByAccount(int accountId)
        {
            context.Database.ExecuteSqlCommand($"DELETE FROM Transactions WHERE AccountId = {accountId}");
        }
    }
}
