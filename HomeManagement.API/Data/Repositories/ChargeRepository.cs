using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeManagement.API.Data.Repositories
{
    public class ChargeRepository : HomeManagement.Data.ChargeRepository, IChargeRepository
    {
        public ChargeRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override void Add(Charge entity)
        {
            using(var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Set<Charge>().Add(entity);

                UpdateBalance(entity);

                dbContext.SaveChanges();

                transaction.Commit();
            }
        }

        public override void Update(Charge entity)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var previousCharge = GetById(entity.Id);

                dbContext.Entry(previousCharge).State = EntityState.Detached;
                dbContext.Attach(entity);
                dbContext.Entry(entity).State = EntityState.Modified;
                dbContext.Set<Charge>().Update(entity);

                UpdateBalance(previousCharge, true);

                UpdateBalance(entity);

                dbContext.SaveChanges();

                transaction.Commit();
            }
        }

        public override void Remove(Charge entity)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Set<Charge>().Remove(entity);

                UpdateBalance(entity, true);

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
