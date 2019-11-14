using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HomeManagement.Contracts.Repositories
{
    public interface IRepository : IDisposable
    {
        int Count();

        void Remove(int id);

        void Commit();
    }

    public interface IRepository<T> : IRepository
    {
        void Add(T entity);

        void Add(IEnumerable<T> entities);

        Task AddAsync(T entity);

        void Remove(T entity);

        void Remove(IEnumerable<T> entities);

        void Update(T entity);

        T GetById(int id);

        T FirstOrDefault();

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll();

        bool Exists(T entity);

        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);

        decimal Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate = null);

        decimal Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null);

        int Count(Expression<Func<T, bool>> predicate);

        IEnumerable<T> Paginate<TOrder>(Func<T, bool> filter, Func<T, TOrder> orderBy, int skip, int take);
    }
}
