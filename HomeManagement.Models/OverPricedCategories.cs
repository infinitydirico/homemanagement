using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class OverPricedCategories
    {
        public OverPricedCategories()
        {
            Categories = new List<OverPricedCategory>();
        }

        [DataMember(Name = "categories")]
        public List<OverPricedCategory> Categories { get; set; }

        public double LowestValue { get; set; }

        public double HighestValue { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (var item in Categories)
            {
                values.Add(item.Category.Name, (item.Value * 1000).ToString());
            }

            return values;
        }
    }
}
