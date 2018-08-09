using HomeManagement.Domain;
using System.Collections.Generic;

namespace HomeManagement.Models
{
    public class TaxPageModel : Page
    {
        public int AccountId { get; set; }
        public List<Tax> Taxes { get; set; }
    }
}
