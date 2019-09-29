using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class TransactionPageModel : Page
    {
        public TransactionPageModel()
        {
            Transactions = new List<TransactionModel>();
        }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "transactions")]
        public List<TransactionModel> Transactions { get; set; }
    }
}
