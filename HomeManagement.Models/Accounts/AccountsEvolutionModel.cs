using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class AccountsEvolutionModel
    {
        public AccountsEvolutionModel()
        {
            this.Accounts = new List<AccountBalanceModel>();
        }

        [DataMember(Name = "accounts")]
        public List<AccountBalanceModel> Accounts { get; set; }

        public int LowestValue { get; set; }

        public int HighestValue { get; set; }
    }

    public class AccountBalanceModel
    {
        public AccountBalanceModel()
        {
            this.BalanceEvolution = new List<MonthBalance>();
        }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "balanceEvolution")]
        public List<MonthBalance> BalanceEvolution { get; set; }
    }

    public class MonthBalance
    {
        [DataMember(Name = "month")]
        public string Month { get; set; }

        [DataMember(Name = "balance")]
        public int Balance { get; set; }
    }
}
