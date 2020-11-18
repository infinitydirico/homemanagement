using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class AccountPageModel : Page
    {
        [DataMember(Name = "userId")]
        public int UserId { get; set; }

        [DataMember(Name = "accounts")]
        public List<AccountModel> Accounts { get; set; }
    }
}
