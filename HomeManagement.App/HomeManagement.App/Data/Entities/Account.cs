using System;

namespace HomeManagement.App.Data.Entities
{
    public class Account : IOfflineEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }

        public bool Measurable { get; set; }

        public AccountType AccountType { get; set; }

        public int CurrencyId { get; set; }

        public int UserId { get; set; }

        public DateTime ChangeStamp { get; set; }

        public DateTime LastApiCall { get; set; }
    }

    public enum AccountType
    {
        Cash,
        Bank,
        CreditCard
    }
}
