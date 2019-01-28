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
        protected DbContext dbContext;

        public BaseRepository(IPlatformContext platformContext)
        {
            this.platformContext = platformContext ?? throw new ArgumentNullException($"{nameof(platformContext)} is null");
            dbContext = platformContext.GetDbContext();
        }

        public IQueryable<T> All => dbContext.Set<T>().AsQueryable<T>();

        public virtual void Add(T entity)
        {
            dbContext.Set<T>().Add(entity);

            dbContext.SaveChanges();
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

            dbContext.SaveChanges();
        }

        public virtual void Remove(int id)
        {
            var entity = GetById(id);

            dbContext.Set<T>().Remove(entity);

            dbContext.SaveChanges();
        }

        public decimal Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate = null) =>
            predicate == null ? dbContext.Set<T>().Sum(selector) : dbContext.Set<T>().Where(predicate).Sum(selector);

        public decimal Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null) =>
            predicate == null ? dbContext.Set<T>().Sum(selector) : dbContext.Set<T>().Where(predicate).Sum(selector);

        public virtual void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);

            dbContext.SaveChanges();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate) => dbContext.Set<T>().Where(predicate).ToList();
    }
}
