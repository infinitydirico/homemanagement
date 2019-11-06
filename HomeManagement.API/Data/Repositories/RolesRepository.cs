using HomeManagement.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HomeManagement.Data;

namespace HomeManagement.API.Data.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private IPlatformContext platformContext;

        public RolesRepository(IPlatformContext platformContext)
        {
            this.platformContext = platformContext ?? throw new ArgumentNullException($"{nameof(platformContext)} is null");
        }

        public IQueryable<IdentityRole<string>> All => platformContext.GetDbContext().Set<IdentityRole<string>>().AsQueryable();

        public void Add(IdentityRole<string> entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<IdentityRole<string>>().Add(entity);
        }

        public int Count(Expression<Func<IdentityRole<string>, bool>> predicate) => All.Count(predicate);

        public int Count() => All.Count();

        public IdentityRole<string> FirstOrDefault() => All.FirstOrDefault();

        public IdentityRole<string> FirstOrDefault(Expression<Func<IdentityRole<string>, bool>> predicate) => All.FirstOrDefault(predicate);

        public IEnumerable<IdentityRole<string>> GetAll() => All.ToList();

        public void Remove(IdentityRole<string> entity)
        {
            var dbContext = platformContext.GetDbContext();

            var role = FirstOrDefault(x => x.Id.Equals(entity.Id));

            dbContext.Set<IdentityRole<string>>().Remove(role);
        }

        public void Update(IdentityRole<string> entity)
        {
            var dbContext = platformContext.GetDbContext();

            var role = FirstOrDefault(x => x.Id.Equals(entity.Id));

            dbContext.Set<IdentityRole<string>>().Update(role);
        }

        public void Commit()
        {
            platformContext.Commit();
        }

        public IEnumerable<IdentityRole<string>> Where(Expression<Func<IdentityRole<string>, bool>> predicate) => All.Where(predicate);

        #region Unimplemented methods

        public IdentityRole<string> GetById(int id)
            => throw new NotImplementedException("Identity Roles does not implement an id of type integer");

        public void Remove(int id)
            => throw new NotImplementedException("Identity Roles does not implement an id of type integer");

        public decimal Sum(Expression<Func<IdentityRole<string>, int>> selector, Expression<Func<IdentityRole<string>, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public decimal Sum(Expression<Func<IdentityRole<string>, decimal>> selector, Expression<Func<IdentityRole<string>, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(IdentityRole<string> entity)
        {
            throw new NotImplementedException();
        }
        public IDbTransaction CreateTransaction()
        {
            throw new NotImplementedException();
        }

        public bool Exists(IdentityRole<string> entity)
        {
            throw new NotImplementedException();
        }

        public void Add(IEnumerable<IdentityRole<string>> entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(IEnumerable<IdentityRole<string>> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IdentityRole<string>> Paginate<TOrder>(Func<IdentityRole<string>, bool> filter, Func<IdentityRole<string>, TOrder> orderBy, int skip, int take)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public interface IRolesRepository : IRepository<IdentityRole<string>>
    {

    }
}
