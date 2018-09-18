using System.Linq;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Accounts
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountStatisticsController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IChargeRepository chargeRepository;
        private readonly IAccountMapper accountMapper;

        public AccountStatisticsController(IAccountRepository accountRepository,
            IChargeRepository chargeRepository,
            IAccountMapper accountMapper)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.accountMapper = accountMapper;
        }

        [HttpGet("{id}/overall")]
        public IActionResult Overall(int id)
        {
            if (id < 0) return BadRequest();

            if (accountRepository.GetById(id) == null) return NotFound();

            var totalCharges = chargeRepository.All.Count(c => c.AccountId.Equals(id));

            var incomeCharges = chargeRepository.All.Count(c => c.AccountId.Equals(id) && c.ChargeType == (int)ChargeType.Income);

            var expensesCharges = chargeRepository.All.Count(c => c.AccountId.Equals(id) && c.ChargeType == ChargeType.Expense);

            return Ok(new AccountOverviewModel
            {
                TotalCharges = totalCharges,
                ExpneseCharges = expensesCharges,
                IncomeCharges = incomeCharges
            });
        }
    }
}