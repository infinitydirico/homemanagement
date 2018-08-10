using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace HomeManagement.API.Throttle
{
    public class BlackList : BaseList
    {
        protected Timer timer = new Timer(60 * 1000);

        #region Singleton
        private static readonly BlackList instance = new BlackList();
        private readonly static object Locker = new object();

        private BlackList()
        {
            timer.Elapsed += CheckBannedClients;
            timer.Start();
        }

        public static BlackList Instance
        {
            get
            {
                lock (Locker)
                {
                    return instance;
                }
            }
        }
        #endregion

        public event EventHandler<ClientRequestEventArgs> OnUnBannedClientRequest;

        private void CheckBannedClients(object sender, ElapsedEventArgs e)
        {
            List<ClientRequest> clientsToUnban = new List<ClientRequest>();
            var bannedClients = listedClients.Where(x => (DateTime.Now - x.BanEnding).TotalSeconds > 30 && x.IsBanned);

            foreach (var request in bannedClients)
            {
                request.RemoveBan();
                clientsToUnban.Add(request);
                OnUnBannedClientRequest.Invoke(this, new ClientRequestEventArgs(request));
            }

            foreach (var unban in clientsToUnban)
            {
                Remove(unban);
            }
        }

        public bool IsIpBanned(string ip)
        {
            return listedClients.FirstOrDefault(x => x.Ip.Equals(ip)) != null;
        }

        public void Ban(ClientRequest clientRequest)
        {
            clientRequest.Ban();
            Add(clientRequest);
        }
    }
}
