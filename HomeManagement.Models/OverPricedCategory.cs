using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class OverPricedCategory
    {
        [DataMember(Name = "category")]
        public CategoryModel Category { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }
    }
}
