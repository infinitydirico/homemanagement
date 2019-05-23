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

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
            => dbContext.Set<T>().FirstOrDefault(predicate);

        public bool Any(Expression<Func<T, bool>> predicate)
            => dbContext.Set<T>().Any(predicate);

        public int Count() => dbContext.Set<T>().Count();

        public int Count(Expression<Func<T, bool>> predicate) => dbContext.Set<T>().Count(predicate);

        public bool HasRecords() => Count() > 0;

        public IEnumerable<T> Skip(int skip) => dbContext.Set<T>().Skip(skip);

        public IEnumerable<T> Take(int take) => dbContext.Set<T>().Take(take);

        public IEnumerable<T> Where(Func<T, bool> predicate) => dbContext.Set<T>().Where(predicate);

        public IOrderedEnumerable<T> OrderByDescending<TKey>(Func<T, TKey> keySelector) 
            => dbContext.Set<T>().OrderByDescending(keySelector);

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
    }
}
