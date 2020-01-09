using System.Collections.Concurrent;
using System.Linq;

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
            return clients.FirstOrDefault(x => x.Ip.Equals(ip));
        }
    }
}
