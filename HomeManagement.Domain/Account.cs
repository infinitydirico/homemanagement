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

        public int Id { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }

        public bool ExcludeFromStatistics { get; set; }

        public AccountType AccountType { get; set; }

        public Currency Currency { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public virtual List<Charge> Charges { get; set; }

        public virtual List<Tax> Taxes { get; set; }
    }

    public enum AccountType
    {
        Cash,
        Bank,
        CreditCard
    }

    public enum Currency
    {
        USD,
        ARS,
        EUR
    }
}
