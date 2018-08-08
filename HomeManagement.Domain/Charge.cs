using System;
using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Domain
{
    public class Charge : BaseEntity//, IExportable
    {
        //IList<string> exportableHeaders = new List<string>
        //{
        //    nameof(Name),
        //    nameof(Price),
        //    nameof(Date),
        //    nameof(ChargeType),
        //    nameof(CategoryName)
        //};

        [Required]
        [MaxLength(250)]
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