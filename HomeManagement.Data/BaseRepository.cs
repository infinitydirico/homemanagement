using HomeManagement.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HomeManagement.Data
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class
    {
        protected IPlatformContext platformContext;

        public BaseRepository(IPlatformContext platformContext)
        {
            this.platformContext = platformContext ?? throw new ArgumentNullException($"{nameof(platformContext)} is null");
        }

        public IQueryable All => throw new NotImplementedException();

        public void Add(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Add(entity);

            dbContext.SaveChanges();
        }

        public async Task AddAsync(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            await dbContext.Set<T>().AddAsync(entity);

            await dbContext.SaveChangesAsync();
        }

        public T FirstOrDefault() => platformContext.GetDbContext().Set<T>().FirstOrDefault();

        public T FirstOrDefault(Expression<Func<T, bool>> predicate) => platformContext.GetDbContext().Set<T>().FirstOrDefault(predicate);

        public IEnumerable<T> GetAll() => platformContext.GetDbContext().Set<T>().ToList();

        public abstract T GetById(int id);

        public void Remove(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Remove(entity);

            dbContext.SaveChanges();
        }

        public void Remove(int id)
        {
            var dbContext = platformContext.GetDbContext();

            var entity = GetById(id);

            dbContext.Set<T>().Remove(entity);

            dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Update(entity);

            dbContext.SaveChanges();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate) => platformContext.GetDbContext().Set<T>().Where(predicate).ToList();
    }
}
