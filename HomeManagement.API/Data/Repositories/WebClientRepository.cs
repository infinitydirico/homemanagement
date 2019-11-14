using HomeManagement.API.Data.Entities;
using HomeManagement.Contracts.Repositories;
using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.API.Data.Repositories
{
    public interface IWebClientRepository : IRepository<WebClient>
    {
        WebClient GetByIp(string ip);
    }

    public class WebClientRepository : BaseRepository<WebClient>, IWebClientRepository
    {
        public WebClientRepository(DbContext context) : base(context)
        {
        }

        public override bool Exists(WebClient entity) => GetById(entity.Id) != null;

        public override WebClient GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));

        public WebClient GetByIp(string ip) => FirstOrDefault(x => x.Ip.Equals(ip));
    }
}
