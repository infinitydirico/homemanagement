using System;
using HomeManagement.Core.Extensions;

namespace HomeManagement.API.Data.Entities
{
    public class WebClient
    {
        public int Id { get; set; }

        public string Ip { get; set; }

        public string Browser { get; set; }

        public int RequestCount { get; set; }

        public DateTime Issued { get; set; }

        public DateTime RequestCountExpires { get; set; }

        public DateTime LastRequest { get; set; }

        public int BanCount { get; set; }

        public DateTime BanEnding { get; set; }

        public bool IsBanned { get; set; }

        public void IncrementCounter()
        {
            RequestCount++;
            LastRequest = DateTime.Now;
        }

        public void Reset()
        {
            RequestCount = 0;
            RequestCountExpires = RequestCountExpires.AddMinutes(1);
        }

        public void Ban()
        {
            BanCount++;
            var factor = BanCount.Fibonacci();
            BanEnding = BanEnding.AddMinutes(factor);
            IsBanned = true;
        }

        public void RemoveBan()
        {
            IsBanned = false;
        }

        public static WebClient Create(string ip) => new WebClient
        {
            Ip = ip,
            RequestCount = 0,
            BanEnding = DateTime.Now,
            RequestCountExpires = DateTime.Now,
            Issued = DateTime.Now,
        };
    }
}
