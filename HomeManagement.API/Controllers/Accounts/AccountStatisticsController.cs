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
    public class AccountStatisticsController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;

        public AccountStatisticsController(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
        }

        [HttpGet("{id}/overall")]
        public IActionResult Overall(int id)
        {
            if (id < 0) return BadRequest();

            if (accountRepository.GetById(id) == null) return NotFound();

            return Ok(new AccountOverviewModel
            {
                TotalTransactions = transactionRepository.All.Count(c => c.AccountId.Equals(id)),
                ExpenseTransactions = transactionRepository.All.Count(c => c.AccountId.Equals(id) && c.TransactionType == TransactionType.Expense),
                IncomeTransactions = transactionRepository.All.Count(c => c.AccountId.Equals(id) && c.TransactionType == (int)TransactionType.Income)
            });
        }

        [HttpGet("getbalance")]
        public IActionResult GetBalance()
        {
            var email = HttpContext.GetEmailClaim();

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(email.Value));

            return Ok(accountRepository.Sum(o => o.Balance.ParseNoDecimals(), o => o.UserId.Equals(user.Id)));
        }

        [HttpGet("incomes")]
        public IActionResult Incomes()
        {
            var email = HttpContext.GetEmailClaim();

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(email.Value));

            //todo missing year compare here and on actions below as well.
            var total = transactionRepository.Sum(c => decimal.Parse(c.Price.ToString()),
                                                    c => c.Account.UserId.Equals(user.Id) &&
                                                    c.Date.Month.Equals(DateTime.Now.Month) &&
                                                    c.Date.Year.Equals(DateTime.Now.Year) &&
                                                    c.TransactionType == TransactionType.Income);

            var previousMonth = transactionRepository.Sum(c => decimal.Parse(c.Price.ToString()),
                                                            c => c.Account.UserId.Equals(user.Id) && 
                                                            c.Date.Month.Equals(c.Date.GetPreviousMonth().Month) &&
                                                            c.Date.Year.Equals(DateTime.Now.Year) &&
                                                            c.TransactionType == TransactionType.Income);

            var percentage = total.CalculatePercentage(previousMonth);

            return Ok(new MetricValueDto
            {
                Total = int.Parse(total.ToString("F0")),
                Percentage = percentage
            });
        }

        [HttpGet("outcomes")]
        public IActionResult Outcomes()
        {
            var email = HttpContext.GetEmailClaim();

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(email.Value));

            var total = transactionRepository.Sum(c => decimal.Parse(c.Price.ToString()),
                                                    c => c.Account.UserId.Equals(user.Id) && 
                                                    c.Date.Month.Equals(DateTime.Now.Month) &&
                                                    c.Date.Year.Equals(DateTime.Now.Year) &&
                                                    c.TransactionType == TransactionType.Expense);

            var previousMonth = transactionRepository.Sum(c => decimal.Parse(c.Price.ToString()), 
                                                            c => c.Account.UserId.Equals(user.Id) && 
                                                            c.Date.Month.Equals(c.Date.GetPreviousMonth().Month) &&
                                                            c.Date.Year.Equals(DateTime.Now.Year) &&
                                                            c.TransactionType == TransactionType.Expense);

            var percentage = total.CalculatePercentage(previousMonth);

            return Ok(new MetricValueDto
            {
                Total = int.Parse(total.ToString("F0")),
                Percentage = percentage
            });
        }
    }
}