using HomeManagement.Business.Contracts;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class MetricsService : IMetricsService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly ICategoryMapper categoryMapper;
        private readonly IUserSessionService userSessionService;

        public MetricsService(IRepositoryFactory repositoryFactory,
            ICategoryMapper categoryMapper,
            IUserSessionService userSessionService)
        {
            this.repositoryFactory = repositoryFactory;
            this.categoryMapper = categoryMapper;
            this.userSessionService = userSessionService;
        }

        public AccountEvolutionModel GetAccountEvolution(int accountId)
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            {
                var model = new AccountEvolutionModel();

                var auhtenticatedUser = userSessionService.GetAuthenticatedUser();

                var months = DateTime.Now.MonthsInYear(DateTime.Now.Month);

                var incomeTransactions = transactionRepository
                    .Where(x => months.Any(m => x.Date.Month.Equals(m)) &&
                            x.AccountId.Equals(accountId) && x.Account.Measurable &&
                            x.TransactionType == TransactionType.Income)
                    .GroupBy(x => x.Date.Month)
                    .Select(z => z.Sum(x => x.Price.ParseNoDecimals()))
                    .ToList();

                var outcomeTransactions = transactionRepository
                    .Where(x => months.Any(m => x.Date.Month.Equals(m)) &&
                            x.AccountId.Equals(accountId) && x.Account.Measurable &&
                            x.TransactionType == TransactionType.Expense)
                    .GroupBy(x => x.Date.Month)
                    .Select(z => z.Sum(x => decimal.ToInt32(x.Price.ParseNoDecimals())))
                    .ToList();

                model.IncomingSeries.AddRange(incomeTransactions);
                model.OutgoingSeries.AddRange(outcomeTransactions);

                var incomeMax = model.IncomingSeries.Max();
                var outcomeMax = model.OutgoingSeries.Max();

                model.HighestValue = incomeMax > outcomeMax ? incomeMax : outcomeMax;
                model.HighestValue += int.Parse((model.HighestValue * 0.25).ToString("F0"));

                var incomeMin = model.IncomingSeries.Min();
                var outcomeMin = model.OutgoingSeries.Min();

                model.LowestValue = incomeMin < outcomeMin ? incomeMin : outcomeMin;

                return model;
            }                
        }

        public AccountOverviewModel GetAccountOverview(int accountId)
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            {
                var accountTransactions = transactionRepository.GetByMeasurableAccount(accountId);

                return new AccountOverviewModel
                {
                    TotalTransactions = accountTransactions.Count(),
                    ExpenseTransactions = accountTransactions.Count(c => c.TransactionType == TransactionType.Expense),
                    IncomeTransactions = accountTransactions.Count(c => c.TransactionType == TransactionType.Income)
                };
            }                
        }

        public AccountsEvolutionModel GetAccountsBalancesEvolution()
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var model = new AccountsEvolutionModel();

                var auhtenticatedUser = userSessionService.GetAuthenticatedUser();

                var accounts = accountRepository.GetAllByUser(auhtenticatedUser.Email);

                int low = 0;
                int high = 0;

                foreach (var account in accounts)
                {
                    if (!account.Measurable) continue;

                    var accountEvoModel = new AccountBalanceModel();

                    for (int i = 1; i <= DateTime.Now.Month; i++)
                    {
                        var incomeTransactions = transactionRepository
                            .Where(x => x.AccountId.Equals(account.Id) &&
                                        x.Account.Measurable &&
                                        x.TransactionType == TransactionType.Income &&
                                        x.Date.Month.Equals(i) &&
                                        x.Date.Year.Equals(DateTime.Now.Year))
                            .Sum(x => x.Price.ParseNoDecimals());

                        var outcomeTransactions = transactionRepository
                            .Where(x => x.AccountId.Equals(account.Id) &&
                                        x.Account.Measurable &&
                                        x.TransactionType == TransactionType.Expense &&
                                        x.Date.Month.Equals(i) &&
                                        x.Date.Year.Equals(DateTime.Now.Year))
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
        }

        public MetricValueDto GetIncomesMetric()
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            {
                var user = userSessionService.GetAuthenticatedUser();

                var previousMonth = DateTime.Now.GetPreviousMonth();

                var total = transactionRepository.SumBy(user.Id, TransactionType.Income, DateTime.Now);
                var previous = transactionRepository.SumBy(user.Id, TransactionType.Income, previousMonth);

                var percentage = total.CalculatePercentage(previous);

                return new MetricValueDto
                {
                    Total = int.Parse(total.ToString("F0")),
                    Percentage = percentage
                };
            }
        }

        public MetricValueDto GetOutcomesMetric()
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            {
                var user = userSessionService.GetAuthenticatedUser();
                var previousMonth = DateTime.Now.GetPreviousMonth();

                var total = transactionRepository.SumBy(user.Id, TransactionType.Expense, DateTime.Now);
                var previous = transactionRepository.SumBy(user.Id, TransactionType.Expense, previousMonth);

                var percentage = total.CalculatePercentage(previous);

                return new MetricValueDto
                {
                    Total = int.Parse(total.ToString("F0")),
                    Percentage = percentage
                };
            }
        }

        public decimal GetUserBalance()
        {
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var user = userSessionService.GetAuthenticatedUser();

                return accountRepository.Sum(o => o.Balance.ParseNoDecimals(), o => o.UserId.Equals(user.Id));
            }
        }

        public IEnumerable<MonthlyCategory> TopTransactionsByAccountAndMonth(int accountId, int month)
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            {
                var auhtenticatedUser = userSessionService.GetAuthenticatedUser();

                if (month.Equals(default(int)))
                {
                    month = DateTime.Now.Month;
                }

                var userCategories = categoryRepository.GetActiveUserCategories(auhtenticatedUser.Email);

                var transactions = transactionRepository
                            .Where(x => x.AccountId.Equals(accountId) &&
                                        x.Account.Measurable &&
                                        x.Date.Month.Equals(month) &&
                                        x.Date.Year.Equals(DateTime.Now.Year) &&
                                        x.TransactionType == TransactionType.Expense &&
                                        x.Category.Measurable);

                var accountTransactions = transactions
                            .GroupBy(x => x.CategoryId)
                            .Select(x => new MonthlyCategory
                            {
                                Category = categoryMapper.ToModel(userCategories.First(uc => uc.Id.Equals(x.First().CategoryId))),
                                Price = x.Sum(d => d.Price)
                            });

                return accountTransactions;
            }
        }

        public OverPricedCategories TopTransactionsByMonth(int month)
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            {
                var auhtenticatedUser = userSessionService.GetAuthenticatedUser();

                if (month.Equals(default))
                {
                    month = DateTime.Now.Month;
                }
                var userCategories = categoryRepository.GetActiveUserCategories(auhtenticatedUser.Email);

                var result = transactionRepository
                    .Where(x => x.Account.User.Email.Equals(auhtenticatedUser.Email) &&
                                x.Account.Measurable &&
                                x.Date.Month.Equals(month) &&
                                x.Date.Year.Equals(DateTime.Now.Year) &&
                                x.TransactionType == TransactionType.Expense &&
                                x.Category.Measurable)
                    .GroupBy(x => x.CategoryId)
                    .Select(x => new OverPricedCategory
                    {
                        Category = categoryMapper.ToModel(userCategories.First(uc => uc.Id.Equals(x.First().CategoryId))),
                        Price = x.Sum(d => d.Price)
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
