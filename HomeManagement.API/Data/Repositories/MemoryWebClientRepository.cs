using HomeManagement.API.Data.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HomeManagement.API.Data.Repositories
{
    public class MemoryWebClientRepository : IWebClientRepository
    {
        public static BlockingCollection<WebClient> clients = new BlockingCollection<WebClient>();

        public IQueryable<WebClient> All => clients.AsQueryable();

        public void Add(WebClient entity)
        {
            clients.Add(entity);
        }

        public void Add(IEnumerable<WebClient> entities)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(WebClient entity)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<WebClient, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IDbTransaction CreateTransaction()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public bool Exists(WebClient entity) => clients.FirstOrDefault(x => x.Id.Equals(entity.Id)) != null;

        public WebClient FirstOrDefault() => clients.FirstOrDefault();

        public WebClient FirstOrDefault(Expression<Func<WebClient, bool>> predicate) => clients.FirstOrDefault(predicate.Compile());

        public IEnumerable<WebClient> GetAll() => clients.ToList();

        public WebClient GetById(int id) => clients.FirstOrDefault(x => x.Id.Equals(id));

        public WebClient GetByIp(string ip) => clients.FirstOrDefault(x => x.Ip.Equals(ip));

        public IEnumerable<WebClient> Paginate<TOrder>(Func<WebClient, bool> filter, Func<WebClient, TOrder> orderBy, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public void Remove(WebClient entity)
        {
            clients.TryTake(out entity);
        }

        public void Remove(int id)
        {
            var entity = GetById(id);
            clients.TryTake(out entity);
        }

        public void Remove(IEnumerable<WebClient> entities)
        {
            throw new NotImplementedException();
        }

        public decimal Sum(Expression<Func<WebClient, int>> selector, Expression<Func<WebClient, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public decimal Sum(Expression<Func<WebClient, decimal>> selector, Expression<Func<WebClient, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public void Update(WebClient entity)
        {
            
        }

        public IEnumerable<WebClient> Where(Expression<Func<WebClient, bool>> predicate) => clients.Where(predicate.Compile()).ToList();
    }
}
