using System;

namespace HomeManagement.App.Data.Entities
{
    public class Charge
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public ChargeType ChargeType { get; set; }

        public int CategoryId { get; set; }

        public int AccountId { get; set; }
    }

    public enum ChargeType
    {
        Income,
        Expense
    }
}
