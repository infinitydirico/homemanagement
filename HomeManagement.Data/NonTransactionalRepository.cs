using System;

namespace HomeManagement.Data
{
    public class NonTransactionalRepository<T> : IWrittableRepository<T>
        where T : class
    {
        protected IPlatformContext platformContext;

        public NonTransactionalRepository(IPlatformContext platformContext)
        {
            this.platformContext = platformContext ?? throw new ArgumentNullException($"{nameof(platformContext)} is null");
        }

        public void Add(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Add(entity);

            dbContext.SaveChanges();
        }

        public void Remove(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Remove(entity);

            dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Update(entity);

            dbContext.SaveChanges();
        }
    }
}
