using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using HomeManagement.Core.Extensions;

namespace HomeManagement.API.Business
{
    public class MetricsService : IMetricsService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategoryMapper categoryMapper;
        private readonly IUserSessionService userSessionService;

        public MetricsService(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            ICategoryMapper categoryMapper,
            IUserSessionService userSessionService)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
            this.categoryMapper = categoryMapper;
            this.userSessionService = userSessionService;
        }

        public AccountEvolutionModel GetAccountEvolution(int accountId)
        {
            var model = new AccountEvolutionModel();

            var auhtenticatedUser = userSessionService.GetAuthenticatedUser();

            var months = DateTime.Now.MonthsInYear(DateTime.Now.Month);

            var query = from c in transactionRepository.All
                        join a in accountRepository.All
                        on c.AccountId equals a.Id
                        join month in months
                        on c.Date.Month equals month
                        where a.Measurable && a.Id.Equals(accountId)
                        select c;

            var incomeTransactions = query
                .Where(x => x.TransactionType == TransactionType.Income)
                .GroupBy(x => x.Date.Month)
                .Select(z => z.Sum(x => x.Price.ParseNoDecimals()))
                .ToList();

            var outcomeTransactions = query
                .Where(x => x.TransactionType == TransactionType.Expense)
                .GroupBy(x => x.Date.Month)
                .Select(z => z.Sum(x => decimal.ToInt32(x.Price.ParseNoDecimals())))
                .ToList();

            model.IncomingSeries.AddRange(incomeTransactions);
            model.OutgoingSeries.AddRange(outcomeTransactions);

            var incomeMax = model.IncomingSeries.Max();
            var outcomeMax = model.OutgoingSeries.Max();

            model.HighestValue = incomeMax > outcomeMax ? incomeMax : outcomeMax;
            model.HighestValue = model.HighestValue + int.Parse((model.HighestValue * 0.25).ToString("F0"));

            var incomeMin = model.IncomingSeries.Min();
            var outcomeMin = model.OutgoingSeries.Min();

            model.LowestValue = incomeMin < outcomeMin ? incomeMin : outcomeMin;

            return model;
        }

        public AccountOverviewModel GetAccountOverview(int accountId)
        {
            var accountTransactions = (from c in transactionRepository.All
                                       join a in accountRepository.All
                                       on c.AccountId equals a.Id
                                       where a.Measurable && a.Id.Equals(accountId)
                                       select c);

            return new AccountOverviewModel
            {
                TotalTransactions = accountTransactions.Count(),
                ExpenseTransactions = accountTransactions.Count(c => c.TransactionType == TransactionType.Expense),
                IncomeTransactions = accountTransactions.Count(c => c.TransactionType == TransactionType.Income)
            };
        }

        public AccountsEvolutionModel GetAccountsBalancesEvolution()
        {
            var model = new AccountsEvolutionModel();

            var auhtenticatedUser = userSessionService.GetAuthenticatedUser();

            var accounts = accountRepository.Where(c => c.UserId.Equals(auhtenticatedUser.Id)).ToList();

            int low = 0;
            int high = 0;

            foreach (var account in accounts)
            {
                if (!account.Measurable) continue;

                var accountEvoModel = new AccountBalanceModel();

                for (int i = 1; i <= DateTime.Now.Month; i++)
                {
                    var accountTransactions = (from c in transactionRepository.All
                                               join a in accountRepository.All
                                               on c.AccountId equals a.Id
                                               where a.Measurable &&
                                                     a.Id.Equals(account.Id) &&
                                                     c.Date.Month.Equals(i) &&
                                                     c.Date.Year.Equals(DateTime.Now.Year)
                                               select c);

                    var incomeTransactions = accountTransactions
                        .Where(x => x.TransactionType == TransactionType.Income)
                        .Sum(x => x.Price.ParseNoDecimals());

                    var outcomeTransactions = accountTransactions
                        .Where(x => x.TransactionType == TransactionType.Expense)
                        .Sum(x => x.Price.ParseNoDecimals());

                    accountEvoModel.AccountId = account.Id;
                    accountEvoModel.AccountName = account.Name;
                    var balance = decimal.ToInt32(incomeTransactions) - decimal.ToInt32(outcomeTransactions);

                    accountEvoModel.BalanceEvolution.Add(balance);

                    if (balance < low)
                    {
                        low = balance;
                    }

                    if (balance > high)
                    {
                        high = balance;
                    }
                }

                model.Balances.Add(accountEvoModel);
            }

            model.HighestValue = high + int.Parse((high * 0.25).ToString("F0"));
            model.LowestValue = low;

            return model;
        }

        public MetricValueDto GetIncomesMetric()
        {
            var total = QueryCurrentMonthTransacctions()
                .Where(x => x.TransactionType.Equals(TransactionType.Income))
                .Sum(x => decimal.Parse(x.Price.ToString()));

            var previous = QueryPreviousMonthTransacctions()
                .Where(x => x.TransactionType.Equals(TransactionType.Income))
                .Sum(x => decimal.Parse(x.Price.ToString()));

            var percentage = total.CalculatePercentage(previous);

            return new MetricValueDto
            {
                Total = int.Parse(total.ToString("F0")),
                Percentage = percentage
            };
        }

        public MetricValueDto GetOutcomesMetric()
        {
            var total = QueryCurrentMonthTransacctions()
                .Where(x => x.TransactionType.Equals(TransactionType.Expense))
                .Sum(x => decimal.Parse(x.Price.ToString()));

            var previous = QueryPreviousMonthTransacctions()
                .Where(x => x.TransactionType.Equals(TransactionType.Expense))
                .Sum(x => decimal.Parse(x.Price.ToString()));

            var percentage = total.CalculatePercentage(previous);

            return new MetricValueDto
            {
                Total = int.Parse(total.ToString("F0")),
                Percentage = percentage
            };
        }

        public decimal GetUserBalance()
        {
            var user = userSessionService.GetAuthenticatedUser();

            return accountRepository.Sum(o => o.Balance.ParseNoDecimals(), o => o.UserId.Equals(user.Id));
        }

        public IEnumerable<MonthlyCategory> TopTransactionsByAccountAndMonth(int accountId, int month)
        {
            var auhtenticatedUser = userSessionService.GetAuthenticatedUser();

            if (month.Equals(default(int)))
            {
                month = DateTime.Now.Month;
            }

            var result = (from c in transactionRepository.All
                          join a in accountRepository.All
                          on c.AccountId equals a.Id
                          join ca in categoryRepository.All
                          on c.CategoryId equals ca.Id
                          where a.Measurable &&
                                a.Id.Equals(accountId) &&
                                c.Date.Month.Equals(month) &&
                                c.Date.Year.Equals(DateTime.Now.Year) &&
                                c.TransactionType == TransactionType.Expense &&
                                ca.Measurable
                          select new { c, ca })
                     .GroupBy(x => x.c.CategoryId)
                     .Select(x => new MonthlyCategory
                     {
                         Category = categoryMapper.ToModel(x.First().ca),
                         Price = x.Sum(d => d.c.Price)
                     })
                     .ToList();

            return result;
        }

        public OverPricedCategories TopTransactionsByMonth(int month)
        {
            var auhtenticatedUser = userSessionService.GetAuthenticatedUser();

            if (month.Equals(default(int)))
            {
                month = DateTime.Now.Month;
            }

            var result = (from transaction in transactionRepository.All
                          join account in accountRepository.All
                          on transaction.AccountId equals account.Id
                          join user in userRepository.All
                          on account.UserId equals user.Id
                          join category in categoryRepository.All
                          on transaction.CategoryId equals category.Id
                          where user.Email.Equals(auhtenticatedUser.Email)
                                   && transaction.TransactionType.Equals(TransactionType.Expense)
                                   && transaction.Date.Month.Equals(month)
                                   && transaction.Date.Year.Equals(DateTime.Now.Year)
                                   && account.Measurable
                                   && category.Measurable
                          select new { Transaction = transaction, Category = category })
                         .GroupBy(x => x.Category.Id)
                         .Select(x => new OverPricedCategory
                         {
                             Category = categoryMapper.ToModel(x.FirstOrDefault().Category),
                             Price = x.Sum(c => c.Transaction.Price)
                         })
                         .ToList();

            var model = new OverPricedCategories
            {
                Categories = result,
                HighestValue = result.Count > 0 ? result.Max(x => x.Price) : 0,
                LowestValue = result.Count > 0 ? result.Min(x => x.Price) : 0
            };

            return model;
        }

        private IQueryable<Transaction> QueryCurrentMonthTransacctions()
        {
            var user = userSessionService.GetAuthenticatedUser();

            var query = transactionRepository.All
                .Where(c => c.Account.UserId.Equals(user.Id) &&
                            c.Date.Month.Equals(DateTime.Now.Month) &&
                            c.Date.Year.Equals(DateTime.Now.Year));
            return query;
        }

        private IQueryable<Transaction> QueryPreviousMonthTransacctions()
        {
            var user = userSessionService.GetAuthenticatedUser();

            var query = transactionRepository.All
                .Where(c => c.Account.UserId.Equals(user.Id) &&
                            c.Date.Month.Equals(c.Date.GetPreviousMonth().Month) &&
                            c.Date.Year.Equals(DateTime.Now.Year));
            return query;
        }
    }

    public interface IMetricsService
    {
        AccountOverviewModel GetAccountOverview(int accountId);

        MetricValueDto GetOutcomesMetric();

        MetricValueDto GetIncomesMetric();

        decimal GetUserBalance();

        AccountsEvolutionModel GetAccountsBalancesEvolution();

        AccountEvolutionModel GetAccountEvolution(int accountId);

        OverPricedCategories TopTransactionsByMonth(int month);

        IEnumerable<MonthlyCategory> TopTransactionsByAccountAndMonth(int accountId, int month);
    }
}
