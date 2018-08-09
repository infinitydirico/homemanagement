using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class AccountsEvolutionModel
    {
        public AccountsEvolutionModel()
        {
            this.Balances = new List<AccountBalanceModel>();
        }

        [DataMember(Name = "balances")]
        public List<AccountBalanceModel> Balances { get; set; }

        public int LowestValue { get; set; }

        public int HighestValue { get; set; }

        public List<Dictionary<string, string>> ToDictionary()
        {
            var list = new List<Dictionary<string, string>>();
            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (var balance in Balances)
            {
                for (int i = 0; i < balance.BalanceEvolution.Count; i++)
                {
                    var item = balance.BalanceEvolution[i] * 1000;
                    var monthName = new DateTime(DateTime.Now.Year, i + 1, 1).ToString("MMMM", new CultureInfo("es"));
                    values.Add(monthName, item.ToString());
                }

                Dictionary<string, string> ret = new Dictionary<string, string>(values.Count,values.Comparer);

                foreach (KeyValuePair<string, string> entry in values)
                {
                    ret.Add(entry.Key, entry.Value);
                }

                list.Add(ret);

                values.Clear();
            }

            return list;
        }
    }

    public class AccountBalanceModel
    {
        public AccountBalanceModel()
        {
            this.BalanceEvolution = new List<int>();
        }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "balanceEvolution")]
        public List<int> BalanceEvolution { get; set; }
    }
}
