using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeManagement.API.Data.Repositories
{
    public interface IChargeRepository : HomeManagement.Data.IChargeRepository
    {
        void Add(Charge charge, bool affectsBalance);

        void Update(Charge charge, bool affectsBalance);

        void Remove(Charge charge, bool affectsBalance);
    }

    public class ChargeRepository : HomeManagement.Data.ChargeRepository, IChargeRepository
    {
        public ChargeRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public void Add(Charge charge, bool affectsBalance)
        {
            using(var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Set<Charge>().Add(charge);

                UpdateBalance(charge);

                dbContext.SaveChanges();

                transaction.Commit();
            }
        }

        public void Update(Charge charge, bool affectsBalance)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var previousCharge = GetById(charge.Id);

                dbContext.Entry(previousCharge).State = EntityState.Detached;
                dbContext.Attach(charge);
                dbContext.Entry(charge).State = EntityState.Modified;
                dbContext.Set<Charge>().Update(charge);

                UpdateBalance(previousCharge, true);

                UpdateBalance(charge);

                dbContext.SaveChanges();

                transaction.Commit();
            }
        }

        public void Remove(Charge charge, bool affectsBalance)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Set<Charge>().Remove(charge);
                dbContext.Entry(charge).State = EntityState.Deleted;

                UpdateBalance(charge, true);

                dbContext.SaveChanges();

                transaction.Commit();
            }
        }

        private void UpdateBalance(Charge c, bool reverse = false)
        {
            var account = dbContext.Set<Account>().First(x => x.Id.Equals(c.AccountId));

            if (reverse)
            {
                c.Price = -c.Price;
            }

            account.Balance = c.ChargeType.Equals(ChargeType.Income) ? account.Balance + c.Price : account.Balance - c.Price;
            dbContext.Set<Account>().Update(account);
        }
    }
}
