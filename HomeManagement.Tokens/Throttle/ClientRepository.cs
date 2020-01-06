using System;
using System.Collections.Concurrent;

namespace HomeManagement.Api.Core.Throttle
{
    public class ClientRepository
    {
        public static BlockingCollection<WebClient> clients = new BlockingCollection<WebClient>();

        public void Add(WebClient client)
        {
            clients.Add(client);
        }

        public WebClient GetByIp(string ip)
        {
            throw new NotImplementedException();
        }
    }
}
