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

        public IQueryable<T> All => platformContext.GetDbContext().Set<T>().AsQueryable<T>();

        public virtual void Add(T entity)
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

        public int Count() => platformContext.GetDbContext().Set<T>().Count();

        public int Count(Expression<Func<T, bool>> predicate) => platformContext.GetDbContext().Set<T>().Count(predicate);

        public abstract bool Exists(T entity);

        public T FirstOrDefault() => platformContext.GetDbContext().Set<T>().FirstOrDefault();

        public T FirstOrDefault(Expression<Func<T, bool>> predicate) => platformContext.GetDbContext().Set<T>().FirstOrDefault(predicate);

        public IEnumerable<T> GetAll() => platformContext.GetDbContext().Set<T>().ToList();

        public abstract T GetById(int id);

        public virtual void Remove(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Remove(entity);

            dbContext.SaveChanges();
        }

        public virtual void Remove(int id)
        {
            var dbContext = platformContext.GetDbContext();

            var entity = GetById(id);

            dbContext.Set<T>().Remove(entity);

            dbContext.SaveChanges();
        }

        public decimal Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate = null) =>
            predicate == null ? platformContext.GetDbContext().Set<T>().Sum(selector) : platformContext.GetDbContext().Set<T>().Where(predicate).Sum(selector);

        public decimal Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null) =>
            predicate == null ? platformContext.GetDbContext().Set<T>().Sum(selector) : platformContext.GetDbContext().Set<T>().Where(predicate).Sum(selector);

        public virtual void Update(T entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<T>().Update(entity);

            dbContext.SaveChanges();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate) => platformContext.GetDbContext().Set<T>().Where(predicate).ToList();
    }
}
