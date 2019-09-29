using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeManagement.API.Data.Repositories
{
    public interface ITransactionRepository : HomeManagement.Data.ITransactionRepository
    {
        void Add(Transaction transaction, bool affectsBalance);

        void Update(Transaction transaction, bool affectsBalance);

        void Remove(Transaction transaction, bool affectsBalance);
    }

    public class TransactionRepository : HomeManagement.Data.TransactionRepository, ITransactionRepository
    {
        public TransactionRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public void Add(Transaction transaction, bool affectsBalance)
        {
            using(var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Set<Transaction>().Add(transaction);

                UpdateBalance(transaction);

                dbContext.SaveChanges();

                dbContextTransaction.Commit();
            }
        }

        public void Update(Transaction transaction, bool affectsBalance)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                var previousTransaction = GetById(transaction.Id);

                dbContext.Entry(previousTransaction).State = EntityState.Detached;
                dbContext.Attach(transaction);
                dbContext.Entry(transaction).State = EntityState.Modified;
                dbContext.Set<Transaction>().Update(transaction);

                UpdateBalance(previousTransaction, true);

                UpdateBalance(transaction);

                dbContext.SaveChanges();

                dbContextTransaction.Commit();
            }
        }

        public void Remove(Transaction transaction, bool affectsBalance)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Set<Transaction>().Remove(transaction);
                dbContext.Entry(transaction).State = EntityState.Deleted;

                UpdateBalance(transaction, true);

                dbContext.SaveChanges();

                dbContextTransaction.Commit();
            }
        }

        private void UpdateBalance(Transaction c, bool reverse = false)
        {
            var account = dbContext.Set<Account>().First(x => x.Id.Equals(c.AccountId));

            if (reverse)
            {
                c.Price = -c.Price;
            }

            account.Balance = c.TransactionType.Equals(TransactionType.Income) ? account.Balance + c.Price : account.Balance - c.Price;
            dbContext.Set<Account>().Update(account);
        }
    }
}
