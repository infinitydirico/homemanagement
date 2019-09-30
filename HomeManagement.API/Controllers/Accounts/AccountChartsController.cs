using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HomeManagement.API.Controllers.Accounts
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountChartsController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategoryMapper categoryMapper;

        public AccountChartsController(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            ICategoryMapper categoryMapper)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
            this.categoryMapper = categoryMapper;
        }


        [HttpGet("{id}/chartbytransactiontype")]
        public IActionResult ChartData(int id)
        {
            var accountTransactions = (from c in transactionRepository.All
                                  join a in accountRepository.All
                                  on c.AccountId equals a.Id
                                  where a.Measurable && a.Id.Equals(id)
                                  select c);

            return Ok(new AccountOverviewModel
            {
                TotalTransactions = accountTransactions.Count(),
                ExpenseTransactions = accountTransactions.Count(c => c.TransactionType == TransactionType.Expense),
                IncomeTransactions = accountTransactions.Count(c => c.TransactionType == (int)TransactionType.Income)
            });
        }

        [HttpGet("accountsevolution")]
        public IActionResult AccountsEvolution()
        {
            var model = new AccountsEvolutionModel();

            var email = HttpContext.GetEmailClaim();

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(email.Value));

            if (user == null) return NotFound();

            var accounts = accountRepository.All.Where(c => c.UserId.Equals(user.Id)).ToList();

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

            return Ok(model);
        }

        [HttpGet("{id}/accountevolution")]
        public IActionResult AccountEvolution(int id)
        {
            var model = new AccountEvolutionModel();

            for (int i = 1; i <= DateTime.Now.Month; i++)
            {
                var accountTransactions = (from c in transactionRepository.All
                                      join a in accountRepository.All
                                      on c.AccountId equals a.Id
                                      where a.Measurable &&
                                            a.Id.Equals(id) &&
                                            c.Date.Month.Equals(i)
                                      select c);

                var incomeTransactions = accountTransactions
                    .Where(x => x.TransactionType == TransactionType.Income)
                    .Sum(x => x.Price.ParseNoDecimals());

                var outcomeTransactions = accountTransactions
                    .Where(x => x.TransactionType == TransactionType.Expense)
                    .Sum(x => x.Price.ParseNoDecimals());

                model.IncomingSeries.Add(decimal.ToInt32(incomeTransactions));
                model.OutgoingSeries.Add(decimal.ToInt32(outcomeTransactions));
            }

            var incomeMax = model.IncomingSeries.Max();
            var outcomeMax = model.OutgoingSeries.Max();

            model.HighestValue = incomeMax > outcomeMax ? incomeMax : outcomeMax;
            model.HighestValue = model.HighestValue + int.Parse((model.HighestValue * 0.25).ToString("F0"));

            var incomeMin = model.IncomingSeries.Min();
            var outcomeMin = model.OutgoingSeries.Min();

            model.LowestValue = incomeMin < outcomeMin ? incomeMin : outcomeMin;

            return Ok(model);
        }

        [HttpGet("toptransactions/{month}")]
        public IActionResult AccountTopTransactions(int month)
        {
            var email = HttpContext.GetEmailClaim();

            if (month.Equals(default(int)))
            {
                month = DateTime.Now.Month;
            }

            //implement a method where it gets all charges of all accounts to the authenticated user that is grouped by categories
            var result = (from transaction in transactionRepository.All
                          join account in accountRepository.All
                          on transaction.AccountId equals account.Id
                          join user in userRepository.All
                          on account.UserId equals user.Id
                          join category in categoryRepository.All
                          on transaction.CategoryId equals category.Id
                          where user.Email.Equals(email.Value)
                                   && transaction.TransactionType.Equals(TransactionType.Expense)
                                   && transaction.Date.Month.Equals(month)
                                   && account.Measurable
                                   && category.Measurable
                          select new { Transaction = transaction, Category = category })
                         .Take(10)
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

            return Ok(model);
        }

        [HttpGet("{id}/toptransactions/{month}")]
        public IActionResult AccountTopTransactions(int id, int month)
        {
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
                           a.Id.Equals(id) &&
                           c.Date.Month.Equals(month) &&
                           c.TransactionType == TransactionType.Expense &&
                           ca.Measurable
                     select new { c, ca })
                     .GroupBy(x => x.c.CategoryId)
                     .Select(x => new { Category = x.FirstOrDefault().ca, Value = x.Sum(d => d.c.Price) })
                     .Take(10)
                     .ToList();

            return Ok(result);
        }
    }
}