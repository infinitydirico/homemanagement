using System.Linq;

namespace HomeManagement.App.Data
{
    public class GenericRepository<T>
        where T : class
    {
        protected MobileAppDbContext dbContext = new MobileAppDbContext();

        public IQueryable<T> All => dbContext.Set<T>().AsQueryable<T>();

        public virtual void Add(T entity)
        {
            dbContext.Set<T>().Add(entity);
        }

        public virtual void Remove(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public virtual void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public void Commit() => dbContext.SaveChanges();
    }
}
