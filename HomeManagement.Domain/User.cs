using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class User : BaseEntity
    {
        public User()
        {
            Accounts = new List<Account>();
            UsersInRoles = new List<UserInRole>();
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual List<Account> Accounts { get; set; }

        public Token Token { get; set; }

        public virtual List<UserInRole> UsersInRoles { get; set; }

        public virtual List<UserCategory> UserCategories { get; set; }

        public virtual List<Share> Shares { get; set; }
    }
}