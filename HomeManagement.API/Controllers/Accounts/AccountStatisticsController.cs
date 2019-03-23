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
        private readonly Data.Repositories.IChargeRepository chargeRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;

        public AccountStatisticsController(IAccountRepository accountRepository,
            Data.Repositories.IChargeRepository chargeRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
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
                TotalCharges = chargeRepository.All.Count(c => c.AccountId.Equals(id)),
                ExpneseCharges = chargeRepository.All.Count(c => c.AccountId.Equals(id) && c.ChargeType == ChargeType.Expense),
                IncomeCharges = chargeRepository.All.Count(c => c.AccountId.Equals(id) && c.ChargeType == (int)ChargeType.Income)
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

            var total = chargeRepository.Sum(c => decimal.Parse(c.Price.ToString()), c => c.Account.UserId.Equals(user.Id) && c.Date.Month.Equals(DateTime.Now.Month) && c.ChargeType == ChargeType.Income);

            var previousMonth = chargeRepository.Sum(c => decimal.Parse(c.Price.ToString()), c => c.Account.UserId.Equals(user.Id) && c.Date.Month.Equals(c.Date.GetPreviousMonth().Month) && c.ChargeType == ChargeType.Income);

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

            var total = chargeRepository.Sum(c => decimal.Parse(c.Price.ToString()), c => c.Account.UserId.Equals(user.Id) && c.Date.Month.Equals(DateTime.Now.Month) && c.ChargeType == ChargeType.Expense);

            var previousMonth = chargeRepository.Sum(c => decimal.Parse(c.Price.ToString()), c => c.Account.UserId.Equals(user.Id) && c.Date.Month.Equals(c.Date.GetPreviousMonth().Month) && c.ChargeType == ChargeType.Expense);

            var percentage = total.CalculatePercentage(previousMonth);

            return Ok(new MetricValueDto
            {
                Total = int.Parse(total.ToString("F0")),
                Percentage = percentage
            });
        }
    }
}