using HomeManagement.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
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
        protected DbContext dbContext => platformContext.GetDbContext();

        public BaseRepository(IPlatformContext platformContext)
        {
            this.platformContext = platformContext ?? throw new ArgumentNullException($"{nameof(platformContext)} is null");
        }

        public virtual void Add(T entity)
        {
            dbContext.Set<T>().Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            dbContext.Set<T>().AddRange(entities);
        }

        public async Task AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);

            await dbContext.SaveChangesAsync();
        }

        public int Count() => dbContext.Set<T>().Count();

        public int Count(Expression<Func<T, bool>> predicate) => dbContext.Set<T>().Count(predicate);

        public abstract bool Exists(T entity);

        public T FirstOrDefault() => dbContext.Set<T>().FirstOrDefault();

        public T FirstOrDefault(Expression<Func<T, bool>> predicate) => dbContext.Set<T>().FirstOrDefault(predicate);

        public IEnumerable<T> GetAll() => dbContext.Set<T>().ToList();

        public abstract T GetById(int id);

        public virtual void Remove(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public virtual void Remove(int id)
        {
            var entity = GetById(id);

            dbContext.Set<T>().Remove(entity);
        }

        public void Remove(IEnumerable<T> entities)
        {
            dbContext.Set<T>().RemoveRange(entities);
        }

        public decimal Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate = null) =>
            predicate == null ? dbContext.Set<T>().Sum(selector) : dbContext.Set<T>().Where(predicate).Sum(selector);

        public decimal Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null) =>
            predicate == null ? dbContext.Set<T>().Sum(selector) : dbContext.Set<T>().Where(predicate).Sum(selector);

        public virtual void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate) => dbContext.Set<T>().Where(predicate).ToList();

        public IEnumerable<T> Paginate<TOrder>(Func<T, bool> filter, Func<T, TOrder> orderBy, int skip, int take)
        {
            var set = dbContext.Set<T>();

            var result = set.Where(filter)
                            .OrderByDescending(orderBy)
                            .Skip(skip)
                            .Take(take)
                            .ToList();

            return result;
        }

        public void Commit()
        {
            platformContext.Commit();
        }
    }
}
