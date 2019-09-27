using System.Collections.Generic;

namespace HomeManagement.Models
{
    public class TransactionsCategoryModel
    {
        public int CategoryId { get; set; }
        public IEnumerable<TransactionModel> Transactions { get; set; }
    }     
}
