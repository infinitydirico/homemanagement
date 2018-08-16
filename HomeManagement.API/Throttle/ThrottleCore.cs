using HomeManagement.API.Data.Entities;
using HomeManagement.API.Data.Repositories;

namespace HomeManagement.API.Throttle
{
    public class ThrottleCore : IThrottleCore
    {
        private readonly IWebClientRepository webClientRepository;

        private ThrottlingOptions options;

        public ThrottleCore(IWebClientRepository webClientRepository) 
            : this(webClientRepository, ThrottlingOptions.GetDefaultOptions())
        {
            this.webClientRepository = webClientRepository;
        }

        public ThrottleCore(IWebClientRepository webClientRepository, ThrottlingOptions options)
        {
            this.options = options;
        }

        public bool CanRequest(string ip)
        {
            var client = webClientRepository.GetByIp(ip);

            if (client != null && client.IsBanned) return false;

            if (client == null)
            {
                CreateNewWebClient(ip);

                return true;
            }
            else
            {
                Update(client);
            }

            if (client.RequestCount > options.MaxRequestsAllowed)
            {
                client.Ban();

                webClientRepository.Update(client);

                return false;
            }

            return true;
        }

        private void CreateNewWebClient(string ip)
        {
            var client = WebClient.Create(ip);

            client.IncrementCounter();

            webClientRepository.Add(client);
        }

        private void Update(WebClient client)
        {
            client.IncrementCounter();

            webClientRepository.Update(client);
        }        
    }
}
