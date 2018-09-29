using System;

namespace HomeManagement.Models
{
    public class ChargeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public ChargeTypeModel ChargeType { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int AccountId { get; set; }
    }

    public enum ChargeTypeModel
    {
        Income,
        Expense
    }
}
