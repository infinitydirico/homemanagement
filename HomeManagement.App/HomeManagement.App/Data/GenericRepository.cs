using Microsoft.EntityFrameworkCore;
using System;
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
                //dbContext.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                Remove(item);
            }
        }

        public void Commit() => dbContext.SaveChanges();
    }
}
