using HomeManagement.Core.Extensions;
using System;

namespace HomeManagement.API.Throttle
{
    public class ClientRequest
    {
        public ClientRequest(string ip)
        {
            Ip = ip;
            RequestsCount = 0;
            Id = ip.ToGuid();
            BanEnding = RequestCountExpires = Issued = DateTime.Now;
        }

        public Guid Id { get; private set; }

        public string Ip { get; private set; }

        public string Browser { get; set; }

        public int RequestsCount { get; private set; }

        public DateTime Issued { get; private set; }

        public DateTime RequestCountExpires { get; private set; }

        public DateTime LastRequest { get; private set; }

        public int BanCounts { get; private set; }

        public DateTime BanEnding { get; private set; }

        public bool IsBanned { get; set; }

        public void IncrementCounter()
        {
            RequestsCount++;
            LastRequest = DateTime.Now;
        }

        public void Reset()
        {
            RequestsCount = 0;
            RequestCountExpires = RequestCountExpires.AddMinutes(1);
        }

        public void Ban()
        {
            BanCounts++;
            var factor = BanCounts.Fibonacci();
            BanEnding = BanEnding.AddMinutes(factor);
            IsBanned = true;
        }

        public void RemoveBan()
        {
            IsBanned = false;
        }
    }
}