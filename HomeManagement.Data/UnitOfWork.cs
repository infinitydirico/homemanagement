using HomeManagement.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace HomeManagement.Data
{
    public class UnitOfWork : IUnitOfWork<DbContext>
    {
        private bool disposedValue = false;

        public UnitOfWork(IPlatformContext platformContext)
        {
            Context = platformContext.CreateContext();
        }

        public DbContext Context { get; }

        public void Commit()
        {
            if (Context.ChangeTracker.HasChanges())
            {
                Context.SaveChanges();
            }
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
