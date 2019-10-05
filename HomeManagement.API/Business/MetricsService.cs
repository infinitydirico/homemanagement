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
    }
}
