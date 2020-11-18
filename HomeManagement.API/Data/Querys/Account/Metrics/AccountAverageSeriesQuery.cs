using System;
using System.Collections.Generic;
using System.Linq;
using HomeManagement.API.Data.Models.Account;

namespace HomeManagement.API.Data.Querys.Account.Metrics
{
    public interface IAccountAverageSeriesQuery
    {
        List<AccountAverageSeriesModel> AccountsAvgSeries(string email);
    }

    public class AccountAverageSeriesQuery : IAccountAverageSeriesQuery
    {
        private readonly WebAppDbContext webAppDbContext;

        public AccountAverageSeriesQuery(WebAppDbContext webAppDbContext)
        {
            this.webAppDbContext = webAppDbContext;
        }
        public List<AccountAverageSeriesModel> AccountsAvgSeries(string email)
        {
            var query = from a in webAppDbContext.Accounts
                        join t in webAppDbContext.Transactions
                        on a.Id equals t.AccountId
                        where t.Date.Year.Equals(DateTime.Now.Year) && 
                                t.Date.Month > (DateTime.Now.Month - 3) &&
                                a.User.Email.Equals(email) &&
                                a.Measurable
                        orderby t.Date.Month
                        select t;

            var querResult = query.ToList();

            var result = querResult
                .GroupBy(x => x.AccountId)
                .Select(x => new AccountAverageSeriesModel
                {
                    AccountId = x.Key,
                    MonthSeries = x
                        .GroupBy(y => y.Date.Month)
                        .Select(z => new MonthSerie
                        {
                            Month = z.Key,
                            Average = z.Average(avg => avg.Price).ToString("0.00")
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }
    }
}
