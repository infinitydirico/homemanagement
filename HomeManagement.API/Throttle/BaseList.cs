using System;
using System.Collections.Concurrent;
using System.Linq;

namespace HomeManagement.API.Throttle
{
    public class BaseList
    {
        protected BlockingCollection<ClientRequest> listedClients = new BlockingCollection<ClientRequest>();

        protected void Add(ClientRequest request)
        {
            listedClients.Add(request);
        }

        protected void Remove(ClientRequest request)
        {
            listedClients.TryTake(out request);
        }

        public ClientRequest Get(string ip)
        {
            return !string.IsNullOrEmpty(ip) ?
                listedClients.FirstOrDefault(o => o.Ip.Equals(ip)) :
                throw new ArgumentNullException($"{nameof(ip)} cannot be null.");
        }
    }
}
