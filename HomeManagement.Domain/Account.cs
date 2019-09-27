using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class Account
    {
        public Account()
        {
            Transactions = new List<Transaction>();
            Taxes = new List<Tax>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }

        public bool Measurable { get; set; }

        public AccountType AccountType { get; set; }

        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

        public virtual List<Tax> Taxes { get; set; }
    }

    public enum AccountType
    {
        Cash,
        Bank,
        CreditCard
    }
}