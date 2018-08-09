using HomeManagement.Domain;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class OverPricedCategory
    {
        [DataMember(Name = "category")]
        public Category Category { get; set; }

        [DataMember(Name = "value")]
        public double Value { get; set; }
    }
}
