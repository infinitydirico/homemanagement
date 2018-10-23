using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class User
    {
        public User()
        {
            Accounts = new List<Account>();
            UsersInRoles = new List<UserInRole>();
            UserCategories = new List<UserCategory>();
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual List<Account> Accounts { get; set; }

        public string Token { get; set; }

        public virtual List<UserInRole> UsersInRoles { get; set; }

        public virtual List<UserCategory> UserCategories { get; set; }

        public virtual List<Share> Shares { get; set; }
    }
}