using System;

namespace HomeManagement.App.Data.Entities
{
    public class Transaction : IOfflineEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }

        public int AccountId { get; set; }

        public DateTime ChangeStamp { get; set; }

        public DateTime LastApiCall { get; set; }

        public bool NeedsUpdate { get; set; }
    }

    public enum TransactionType
    {
        Income,
        Expense
    }
}
