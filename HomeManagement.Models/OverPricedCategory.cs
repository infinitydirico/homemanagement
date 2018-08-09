using HomeManagement.Domain;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class OverPricedCategory
    {
        [DataMember(Name = "category")]
        public Category Category { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }
    }
}
