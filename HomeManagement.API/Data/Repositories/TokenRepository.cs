using HomeManagement.Contracts.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private WebAppDbContext context;

        public TokenRepository(DbContext context)
        {
            this.context = (WebAppDbContext)context;
        }

        public void Add(IdentityUserToken<string> entity)
        {
            context.Set<IdentityUserToken<string>>().Add(entity);
        }

        public void Add(IEnumerable<IdentityUserToken<string>> entities)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(IdentityUserToken<string> entity)
        {
            await context.Set<IdentityUserToken<string>>().AddAsync(entity);
        }

        public bool Exists(IdentityUserToken<string> entity) => FirstOrDefault(x => x.UserId.Equals(entity.UserId)) != null;

        public IdentityUserToken<string> FirstOrDefault()
        {
            return context.Set<IdentityUserToken<string>>().FirstOrDefault();
        }

        public IdentityUserToken<string> FirstOrDefault(Expression<Func<IdentityUserToken<string>, bool>> predicate)
        {
            return context.Set<IdentityUserToken<string>>().FirstOrDefault(predicate);
        }

        public IEnumerable<IdentityUserToken<string>> GetAll()
        {
            return context.Set<IdentityUserToken<string>>().ToList();
        }

        public IdentityUserToken<string> GetById(int id) => FirstOrDefault(x => x.UserId.Equals(id));

        public void Remove(IdentityUserToken<string> entity)
        {
            context.Set<IdentityUserToken<string>>().Remove(entity);
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

        public void Update(IdentityUserToken<string> entity)
        {
            context.Set<IdentityUserToken<string>>().Update(entity);
        }

        public bool UserHasToken(string appId) => FirstOrDefault(x => x.UserId.Equals(appId)) != null;

        public IEnumerable<IdentityUserToken<string>> Where(Expression<Func<IdentityUserToken<string>, bool>> predicate)
        {
            return context.Set<IdentityUserToken<string>>().Where(predicate).ToList();
        }

        public void Commit()
        {
            context.SaveChanges();
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #region Unimplemented methods

        public void Remove(IEnumerable<IdentityUserToken<string>> entities)
        {
            throw new NotImplementedException();
        }

        public decimal Sum(Expression<Func<IdentityUserToken<string>, int>> selector, Expression<Func<IdentityUserToken<string>, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public decimal Sum(Expression<Func<IdentityUserToken<string>, decimal>> selector, Expression<Func<IdentityUserToken<string>, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<IdentityUserToken<string>, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IdentityUserToken<string>> Paginate<TOrder>(Func<IdentityUserToken<string>, bool> filter, Func<IdentityUserToken<string>, TOrder> orderBy, int skip, int take)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
