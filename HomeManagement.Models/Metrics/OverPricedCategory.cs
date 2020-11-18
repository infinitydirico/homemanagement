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

    /// <summary>
    /// When the method value is corrected to return an array of objects as above
    /// this class should be removed
    /// [HttpGet("{id}/topcharges/{month}")]
    /// public IActionResult AccountTopCharges(int id, int month)
    /// </summary>
    public class OverPricedCategory2
    {
        [DataMember(Name = "category")]
        public CategoryModel Category { get; set; }

        [DataMember(Name = "value")]
        public double Value { get; set; }
    }
}
