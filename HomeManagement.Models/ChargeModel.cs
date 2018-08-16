using System;

namespace HomeManagement.Models
{
    public class ChargeModel
    {
        public ChargeModel() { }

        public ChargeModel(int id, string name, int price, DateTime date, ChargeTypeModel chargeType, int categoryId, int accountId)
        {
            Id = id;
            Name = name;
            Price = price;
            Date = date;
            ChargeType = chargeType;
            CategoryId = categoryId;
            AccountId = accountId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

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
