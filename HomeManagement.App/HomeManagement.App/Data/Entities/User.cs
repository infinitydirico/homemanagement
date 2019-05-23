using System;

namespace HomeManagement.App.Data.Entities
{
    public class User : IOfflineEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public DateTime ChangeStamp { get; set; }

        public DateTime LastApiCall { get; set; }

        public bool NeedsUpdate { get; set; }
    }
}
