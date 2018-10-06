using Microsoft.EntityFrameworkCore;
using System;

namespace HomeManagement.Data
{
    public class TransactionalRepository<T> : IWrittableRepository<T>
        where T : class
    {
        protected IPlatformContext platformContext;

        public TransactionalRepository(IPlatformContext platformContext)
        {
            this.platformContext = platformContext ?? throw new ArgumentNullException($"{nameof(platformContext)} is null");
        }

        public void Add(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            //dbContext.Set<T>().Add(entity);

            //dbContext.SaveChanges();
            var currentTransaction = platformContext.GetCurrentTransaction();

            using (var transaction = currentTransaction == null ? dbContext.Database.BeginTransaction() : dbContext.Database.UseTransaction(currentTransaction))
            {
                dbContext.Set<T>().Add(entity);

                //dbContext.SaveChanges();

                //transaction.Commit();
            }
        }

        public void Remove(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Remove(entity);

            dbContext.SaveChanges();
            //using (var transaction = dbContext.Database.BeginTransaction())
            //{
            //    dbContext.Set<T>().Remove(entity);

            //    dbContext.SaveChanges();

            //    transaction.Commit();
            //}
        }

        public void Update(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            //dbContext.Set<T>().Update(entity);

            //dbContext.SaveChanges();
            var currentTransaction = platformContext.GetCurrentTransaction();

            using (var transaction = currentTransaction == null ? dbContext.Database.BeginTransaction() : dbContext.Database.UseTransaction(currentTransaction))
            {
                dbContext.Set<T>().Update(entity);

                //dbContext.SaveChanges();

                //transaction.Commit();
            }
        }
    }
}
