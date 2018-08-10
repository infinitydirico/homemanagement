namespace HomeManagement.API.Throttle
{
    public class ThrottleCore : IThrottleCore
    {
        private ThrottlingOptions options;

        public ThrottleCore() : this(ThrottlingOptions.GetDefaultOptions())
        {
            BlackList.Instance.OnUnBannedClientRequest += BlackList_OnUnBannedClientRequest;
        }

        public ThrottleCore(ThrottlingOptions options)
        {
            this.options = options;
        }

        private void BlackList_OnUnBannedClientRequest(object sender, ClientRequestEventArgs e)
        {
            WhiteList.Instance.Add(e.ClientRequest);
        }

        public bool CanRequest(string ip)
        {
            if (BlackList.Instance.IsIpBanned(ip)) return false;

            WhiteListClient(ip);

            var canRequest = BlackListClient(ip);

            return canRequest;
        }

        private void WhiteListClient(string ip)
        {
            var client = WhiteList.Instance.Get(ip);

            if (client == null) WhiteList.Instance.Add(new ClientRequest(ip));
            else client.IncrementCounter();
        }

        private bool BlackListClient(string ip)
        {
            var client = WhiteList.Instance.Get(ip);

            if (HasExceedCuota(client))
            {
                BlackList.Instance.Ban(client);

                WhiteList.Instance.Remove(client);

                return false;
            }
            return true;
        }

        private bool HasExceedCuota(ClientRequest clientRequest)
        {
            return clientRequest.RequestsCount > options.MaxRequestsAllowed;
        }
    }
}
