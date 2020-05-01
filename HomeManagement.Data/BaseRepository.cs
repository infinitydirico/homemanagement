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
        protected DbContext context;
        protected IPlatformContext platformContext;

        public BaseRepository(DbContext context)
        {
            this.context = context;
        }

        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            context.Set<T>().AddRange(entities);
        }

        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public int Count() => context.Set<T>().Count();

        public int Count(Expression<Func<T, bool>> predicate) => context.Set<T>().Count(predicate);

        public abstract bool Exists(T entity);

        public T FirstOrDefault() => context.Set<T>().FirstOrDefault();

        public T FirstOrDefault(Expression<Func<T, bool>> predicate) => context.Set<T>().FirstOrDefault(predicate);

        public IEnumerable<T> GetAll() => context.Set<T>().ToList();

        public abstract T GetById(int id);

        public virtual void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public virtual void Remove(int id)
        {
            var entity = GetById(id);

            context.Set<T>().Remove(entity);
        }

        public void Remove(IEnumerable<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
        }

        public decimal Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate = null) =>
            predicate == null ? context.Set<T>().Sum(selector) : context.Set<T>().Where(predicate).Sum(selector);

        public decimal Sum(Func<T, decimal> selector, Func<T, bool> predicate = null) =>
            predicate == null ? context.Set<T>().Sum(selector) : context.Set<T>().Where(predicate).Sum(selector);

        public virtual void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate) => context.Set<T>().Where(predicate).ToList();

        public IEnumerable<T> Paginate<TOrder>(Func<T, bool> filter, Func<T, TOrder> orderBy, int skip, int take)
        {
            var set = context.Set<T>();

            var result = set.Where(filter)
                            .OrderByDescending(orderBy)
                            .Skip(skip)
                            .Take(take)
                            .ToList();

            return result;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                context = null;
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
