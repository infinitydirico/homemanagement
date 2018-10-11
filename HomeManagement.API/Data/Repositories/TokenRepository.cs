using HomeManagement.Contracts.Repositories;
using HomeManagement.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HomeManagement.API.Data.Repositories
{
    public interface ITokenRepository : IRepository<IdentityUserToken<string>>
    {
        bool UserHasToken(string appId);

        void Remove(string appUserId);
    }

    public class TokenRepository : ITokenRepository
    {
        private IPlatformContext platformContext;

        public TokenRepository(IPlatformContext platformContext)
        {
            this.platformContext = platformContext ?? throw new ArgumentNullException($"{nameof(platformContext)} is null");
        }

        public IQueryable<IdentityUserToken<string>> All => platformContext.GetDbContext().Set<IdentityUserToken<string>>().AsQueryable();

        public void Add(IdentityUserToken<string> entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<IdentityUserToken<string>>().Add(entity);

            dbContext.SaveChanges();
        }

        public async Task AddAsync(IdentityUserToken<string> entity)
        {
            var dbContext = platformContext.GetDbContext();

            await dbContext.Set<IdentityUserToken<string>>().AddAsync(entity);

            dbContext.SaveChanges();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<IdentityUserToken<string>, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Exists(IdentityUserToken<string> entity) => FirstOrDefault(x => x.UserId.Equals(entity.UserId)) != null;

        public IdentityUserToken<string> FirstOrDefault() => All.FirstOrDefault();

        public IdentityUserToken<string> FirstOrDefault(Expression<Func<IdentityUserToken<string>, bool>> predicate)
            => All.FirstOrDefault(predicate);

        public IEnumerable<IdentityUserToken<string>> GetAll() => platformContext.GetDbContext().Set<IdentityUserToken<string>>().ToList();

        public IdentityUserToken<string> GetById(int id) => FirstOrDefault(x => x.UserId.Equals(id));

        public void Remove(IdentityUserToken<string> entity)
        {
            var dbContext = platformContext.GetDbContext();

            dbContext.Set<IdentityUserToken<string>>().Remove(entity);

            dbContext.SaveChanges();
        }

        public void Remove(int id)
        {
            var entity = GetById(id);

            Remove(entity);
        }

        public void Remove(string appUserId)
        {
            var entity = FirstOrDefault(x => x.UserId.Equals(appUserId));

            Remove(entity);
        }

        public decimal Sum(Expression<Func<IdentityUserToken<string>, int>> selector, Expression<Func<IdentityUserToken<string>, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public decimal Sum(Expression<Func<IdentityUserToken<string>, decimal>> selector, Expression<Func<IdentityUserToken<string>, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public void Update(IdentityUserToken<string> entity)
        {
            var dbContext = platformContext.GetDbContext();
            dbContext.Set<IdentityUserToken<string>>().Update(entity);
            dbContext.SaveChanges();
        }

        public bool UserHasToken(string appId) => FirstOrDefault(x => x.UserId.Equals(appId)) != null;

        public IEnumerable<IdentityUserToken<string>> Where(Expression<Func<IdentityUserToken<string>, bool>> predicate)
            => platformContext.GetDbContext().Set<IdentityUserToken<string>>().Where(predicate).ToList();
    }
}
