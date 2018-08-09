using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class Account
    {
        public Account()
        {
            Charges = new List<Charge>();
            Taxes = new List<Tax>();
        }

        public Account(int id, string name, int balance, bool excludeFormStatistics, bool isCash, int userId)
            :this()
        {
            Id = id;
            Name = name;
            Balance = balance;
            ExcludeFromStatistics = excludeFormStatistics;
            IsCash = isCash;
            UserId = userId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Balance { get; set; }

        public bool ExcludeFromStatistics { get; set; }

        public bool IsCash { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public virtual List<Charge> Charges { get; set; }

        public virtual List<Tax> Taxes { get; set; }
    }
}
