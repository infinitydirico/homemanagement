using HomeManagement.Contracts.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HomeManagement.Data
{
    public abstract class TransactionalRepository<T> : IRepository<T>, IDisposable
        where T : class
    {
        protected IPlatformContext platformContext;

        public TransactionalRepository(IPlatformContext platformContext)
        {
            this.platformContext = platformContext ?? throw new ArgumentNullException($"{nameof(platformContext)} is null");
        }

        public IQueryable<T> All => platformContext.CreateContext().Set<T>().AsQueryable();

        public virtual void Add(T entity)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                dbContext.Set<T>().Add(entity);

                dbContext.SaveChanges();
            }
        }

        public async Task AddAsync(T entity)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                await dbContext.Set<T>().AddAsync(entity);

                await dbContext.SaveChangesAsync();
            }
        }

        public int Count()
        {
            using (var dbContext = platformContext.CreateContext())
            {
                return dbContext.Set<T>().Count();
            }
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                return dbContext.Set<T>().Count(predicate);
            }
        }

        public abstract bool Exists(T entity);

        public T FirstOrDefault()
        {
            using (var dbContext = platformContext.CreateContext())
            {
                return dbContext.Set<T>().FirstOrDefault();
            }
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                return dbContext.Set<T>().FirstOrDefault(predicate);
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var dbContext = platformContext.CreateContext())
            {
                return dbContext.Set<T>().ToList();
            }
        }

        public abstract T GetById(int id);

        public virtual void Remove(T entity)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                dbContext.Set<T>().Remove(entity);

                dbContext.SaveChanges();
            }
        }

        public virtual void Remove(int id)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                var entity = GetById(id);

                dbContext.Set<T>().Remove(entity);

                dbContext.SaveChanges();
            }
        }

        public decimal Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate = null)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                return predicate == null ? dbContext.Set<T>().Sum(selector) : dbContext.Set<T>().Where(predicate).Sum(selector);
            }
        }


        public decimal Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                return predicate == null ? dbContext.Set<T>().Sum(selector) : dbContext.Set<T>().Where(predicate).Sum(selector);
            }
        }

        public virtual void Update(T entity)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                dbContext.Set<T>().Update(entity);

                dbContext.SaveChanges();
            }
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            using (var dbContext = platformContext.CreateContext())
            {
                return dbContext.Set<T>().Where(predicate).ToList();
            }
        }

        public IDbTransaction CreateTransaction()
        {
            return platformContext.CreateContext().Database.BeginTransaction().GetDbTransaction();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TransactionalRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
