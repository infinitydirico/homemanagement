using HomeManagement.Domain;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class ChargesCategoryModel
    {
        public int CategoryId { get; set; }
        public IEnumerable<Charge> Charges { get; set; }
    }     
}
