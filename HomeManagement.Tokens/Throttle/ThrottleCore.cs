namespace HomeManagement.Api.Core.Throttle
{
    public class ThrottleCore : IThrottleCore
    {
        private readonly ClientRepository clientRepository = new ClientRepository();

        private ThrottlingOptions options;

        public ThrottleCore() 
            : this(ThrottlingOptions.GetDefaultOptions())
        {
        }

        public ThrottleCore(ThrottlingOptions options)
        {
            this.options = options;
        }

        public bool CanRequest(string ip)
        {
            var client = clientRepository.GetByIp(ip);

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

                return false;
            }

            return true;
        }

        private void CreateNewWebClient(string ip)
        {
            var client = WebClient.Create(ip);

            client.IncrementCounter();

            clientRepository.Add(client);
        }

        private void Update(WebClient client)
        {
            client.IncrementCounter();
        }        
    }
}
