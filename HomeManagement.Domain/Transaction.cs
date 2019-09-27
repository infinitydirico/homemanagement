using HomeManagement.Contracts;
using System;
using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class Transaction : IExportable
    {
        IList<string> exportableHeaders = new List<string>
        {
            nameof(Name),
            nameof(Price),
            nameof(Date),
            nameof(TransactionType),
            nameof(CategoryName)
        };

        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName => Category?.Name;

        public Category Category { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        public IList<string> GetProperties() => exportableHeaders;
    }

    public enum TransactionType
    {
        Income,
        Expense
    }
}