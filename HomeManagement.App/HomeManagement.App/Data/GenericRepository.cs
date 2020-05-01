using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HomeManagement.App.Data
{
    public class GenericRepository<T>
        where T : class
    {
        protected MobileAppDbContext dbContext = new MobileAppDbContext();

        public T FirstOrDefault(Expression<Func<T, bool>> predicate) => GetDbSet().FirstOrDefault(predicate);

        public T FirstOrDefault() => GetDbSet().FirstOrDefault();

        public bool Any() => GetDbSet().Any();

        public bool Any(Expression<Func<T, bool>> predicate) => GetDbSet().Any(predicate);

        public int Count() => GetDbSet().Count();

        public int Count(Expression<Func<T, bool>> predicate) => GetDbSet().Count(predicate);

        public bool HasRecords() => Count() > 0;

        public IEnumerable<T> Skip(int skip) => GetDbSet().Skip(skip);

        public IEnumerable<T> Take(int take) => GetDbSet().Take(take);

        public IEnumerable<T> Where(Func<T, bool> predicate) => GetDbSet().Where(predicate);

        public IEnumerable<T> GetAll() => GetDbSet().ToList();

        public IOrderedEnumerable<T> OrderByDescending<TKey>(Func<T, TKey> keySelector) 
            => GetDbSet().OrderByDescending(keySelector);

        public virtual void Add(T entity)
        {
            dbContext.Set<T>().Add(entity);
            dbContext.Entry(entity).State = EntityState.Added;
        }

        public virtual void Remove(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void RemoveAll()
        {
            foreach (var item in dbContext.Set<T>().ToList())
            {
                Remove(item);
            }
        }

        public void Commit() => dbContext.SaveChanges();

        private IQueryable<T> GetDbSet() => dbContext.Set<T>().AsNoTracking();
    }
}
