using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HomeManagement.Contracts.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> All { get; }

        void Add(T entity);

        Task AddAsync(T entity);

        void Remove(T entity);

        void Remove(int id);

        void Update(T entity);

        T GetById(int id);

        T FirstOrDefault();

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll();

        bool Exists(T entity);

        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);

        decimal Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate = null);
    }
}
