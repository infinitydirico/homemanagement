using System;

namespace HomeManagement.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork
    {
        TContext Context { get; }
    }
}
