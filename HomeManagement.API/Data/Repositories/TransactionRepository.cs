using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeManagement.API.Data.Repositories
{
    public interface ITransactionRepository : HomeManagement.Data.ITransactionRepository
    {
        void Add(Transaction charge, bool affectsBalance);

        void Update(Transaction charge, bool affectsBalance);

        void Remove(Transaction charge, bool affectsBalance);
    }

    public class TransactionRepository : HomeManagement.Data.TransactionRepository, ITransactionRepository
    {
        public TransactionRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public void Add(Transaction charge, bool affectsBalance)
        {
            using(var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Set<Transaction>().Add(charge);

                UpdateBalance(charge);

                dbContext.SaveChanges();

                transaction.Commit();
            }
        }

        public void Update(Transaction charge, bool affectsBalance)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var previousCharge = GetById(charge.Id);

                dbContext.Entry(previousCharge).State = EntityState.Detached;
                dbContext.Attach(charge);
                dbContext.Entry(charge).State = EntityState.Modified;
                dbContext.Set<Transaction>().Update(charge);

                UpdateBalance(previousCharge, true);

                UpdateBalance(charge);

                dbContext.SaveChanges();

                transaction.Commit();
            }
        }

        public void Remove(Transaction charge, bool affectsBalance)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Set<Transaction>().Remove(charge);
                dbContext.Entry(charge).State = EntityState.Deleted;

                UpdateBalance(charge, true);

                dbContext.SaveChanges();

                transaction.Commit();
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
