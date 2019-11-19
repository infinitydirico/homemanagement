using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class User
    {
        public User()
        {
            Accounts = new List<Account>();
            Categories = new List<Category>();
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public virtual List<Category> Categories { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }
}