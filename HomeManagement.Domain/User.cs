using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeManagement.Domain
{
    public class User : BaseEntity
    {
        public User()
        {
            Accounts = new List<Account>();
            UsersInRoles = new List<UserInRole>();
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual List<Account> Accounts { get; set; }

        [NotMapped]
        public Token Token { get; set; }

        public virtual List<UserInRole> UsersInRoles { get; set; }

        public virtual List<UserCategory> UserCategories { get; set; }

        public virtual List<Share> Shares { get; set; }
    }
}