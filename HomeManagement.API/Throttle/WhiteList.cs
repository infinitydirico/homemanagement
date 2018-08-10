using System;
using System.Linq;
using System.Timers;

namespace HomeManagement.API.Throttle
{
    public class WhiteList : BaseList
    {
        protected Timer timer = new Timer(60 * 1000);

        #region Singleton
        private static readonly WhiteList instance = new WhiteList();
        private readonly static object Locker = new object();

        private WhiteList()
        {
            timer.Elapsed += CheckListedClients;
            timer.Start();
        }

        public static WhiteList Instance
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

        protected virtual void CheckListedClients(object sender, ElapsedEventArgs e)
        {
            CheckForExpiredClients();
            CheckForExpiredCounts();
        }

        private void CheckForExpiredCounts()
        {
            var expiredCounts = listedClients.Where(x => (DateTime.Now - x.RequestCountExpires).TotalSeconds > 30);

            foreach (var request in expiredCounts)
            {
                request.Reset();
            }
        }

        private void CheckForExpiredClients()
        {
            var expiredClients = listedClients.Where(x => (DateTime.Now - x.LastRequest).TotalMinutes > 10);

            foreach (var request in expiredClients)
            {
                var r = request;
                listedClients.TryTake(out r);
            }
        }

        public new void Add(ClientRequest clientRequest)
        {
            base.Add(clientRequest);
        }

        public new void Remove(ClientRequest clientRequest)
        {
            base.Remove(clientRequest);
        }

    }
}
