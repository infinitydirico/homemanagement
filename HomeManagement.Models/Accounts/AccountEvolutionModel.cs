using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class AccountEvolutionModel
    {
        public AccountEvolutionModel()
        {
            OutgoingSeries = new List<int>();
            IncomingSeries = new List<int>();
        }

        [DataMember(Name = "outgoingSeries")]
        public List<int> OutgoingSeries { get; set; }

        [DataMember(Name = "IncomingSeries")]
        public List<int> IncomingSeries { get; set; }

        public int LowestValue { get; set; }

        public int HighestValue { get; set; }

        public List<Dictionary<string, string>> ToDictionary()
        {
            var value = new List<Dictionary<string, string>>();

            value.Add(GetSeries(IncomingSeries.ToArray()));
            value.Add(GetSeries(OutgoingSeries.ToArray()));

            return value;
        }

        private Dictionary<string, string> GetSeries(int[] serie)
        {
            var value = new Dictionary<string, string>();

            for (int i = 0; i < serie.Length; i++)
            {
                var item = serie[i];
                var monthName = new DateTime(DateTime.Now.Year, i + 1, 1).ToString("MMMM", new CultureInfo("es"));

                value.Add(monthName, item.ToString());
            }

            return value;
        }
    }
}
