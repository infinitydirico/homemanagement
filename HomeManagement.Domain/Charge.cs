using System;

namespace HomeManagement.Domain
{
    public class Charge //, IExportable
    {
        //IList<string> exportableHeaders = new List<string>
        //{
        //    nameof(Name),
        //    nameof(Price),
        //    nameof(Date),
        //    nameof(ChargeType),
        //    nameof(CategoryName)
        //};

        public Charge() { }

        public Charge(int id, string name, int price, DateTime date, ChargeType chargeType, int categoryId, int accountId)
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

        public ChargeType ChargeType { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName => Category?.Name;

        public Category Category { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        //public IList<string> GetProperties() => exportableHeaders;
    }

    public enum ChargeType
    {
        Incoming,
        Outgoing
    }
}