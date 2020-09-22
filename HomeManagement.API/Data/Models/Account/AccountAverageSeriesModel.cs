using System.Collections.Generic;

namespace HomeManagement.API.Data.Models.Account
{
    public class AccountAverageSeriesModel
    {
        public int AccountId { get; set; }

        public List<MonthSerie> MonthSeries { get; set; }
    }

    public class MonthSerie
    {
        public int Month { get; set; }

        public string Average { get; set; }
    }
}
