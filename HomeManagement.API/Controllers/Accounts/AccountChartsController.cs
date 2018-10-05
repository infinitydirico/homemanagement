using System;
using System.IdentityModel.Tokens.Jwt;
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
    public class AccountChartsController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IChargeRepository chargeRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;

        public AccountChartsController(IAccountRepository accountRepository,
            IChargeRepository chargeRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
        }


        [HttpGet("{id}/chartbychargetype")]
        public IActionResult ChartData(int id)
        {
            var totalCharges = chargeRepository.Count(c => c.AccountId.Equals(id));

            var incomingCharges = chargeRepository.Count(c => c.AccountId.Equals(id) && c.ChargeType == (int)ChargeType.Income);

            var outgoingCharges = chargeRepository.Count(c => c.AccountId.Equals(id) && c.ChargeType == ChargeType.Expense);

            return Ok(new AccountOverviewModel
            {
                TotalCharges = totalCharges,
                ExpneseCharges = outgoingCharges,
                IncomeCharges = incomingCharges
            });
        }

        [HttpGet("accountsevolution")]
        public IActionResult AccountsEvolution()
        {
            var model = new AccountsEvolutionModel();

            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(claim.Value));

            if (user == null) return NotFound();

            var accounts = accountRepository.All.Where(c => c.UserId.Equals(user.Id)).ToList();

            int low = 0;
            int high = 0;

            foreach (var account in accounts)
            {
                var accountEvoModel = new AccountBalanceModel();

                for (int i = 1; i <= DateTime.Now.Month; i++)
                {
                    var incomingCharges = chargeRepository
                                       .Sum(c => int.Parse(c.Price.ToString()), c => c.AccountId.Equals(account.Id)
                                                    && c.ChargeType == (int)ChargeType.Income
                                                    && c.Date.Month.Equals(i));

                    var outgoingCharges = chargeRepository
                                        .Sum(c => int.Parse(c.Price.ToString()), c => c.AccountId.Equals(account.Id)
                                                    && c.ChargeType == ChargeType.Expense
                                                    && c.Date.Month.Equals(i));

                    accountEvoModel.AccountId = account.Id;
                    accountEvoModel.AccountName = account.Name;
                    var balance = decimal.ToInt32(incomingCharges) - decimal.ToInt32(outgoingCharges);

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

            model.HighestValue = high;
            model.LowestValue = low;

            return Ok(model);
        }

        [HttpGet("{id}/accountevolution")]
        public IActionResult AccountEvolution(int id)
        {
            var model = new AccountEvolutionModel();

            for (int i = 1; i <= DateTime.Now.Month; i++)
            {
                var incomingCharges = chargeRepository
                                        .Sum(c => int.Parse(c.Price.ToString()), c => c.AccountId.Equals(id)
                                                     && c.ChargeType == (int)ChargeType.Income
                                                     && c.Date.Month.Equals(i));

                var outgoingCharges = chargeRepository
                                        .Sum(c => int.Parse(c.Price.ToString()), c => c.AccountId.Equals(id)
                                                     && c.ChargeType == ChargeType.Expense
                                                     && c.Date.Month.Equals(i));

                model.IncomingSeries.Add(decimal.ToInt32(incomingCharges));
                model.OutgoingSeries.Add(decimal.ToInt32(outgoingCharges));
            }

            return Ok(model);
        }

        [HttpGet("{id}/topcharges/{month}")]
        public IActionResult AccountTopCharges(int id, int month)
        {
            if (month.Equals(default(int)))
            {
                month = DateTime.Now.Month;
            }

            var charges = chargeRepository.All.Where(c => c.AccountId.Equals(id)
                                                     && c.ChargeType == ChargeType.Expense
                                                     && c.Date.Month.Equals(month)).ToList();

            var result = charges.GroupBy(c => c.CategoryId)
                                .Select(s => new { Category = categoryRepository.GetById(s.FirstOrDefault().CategoryId), Value = s.Sum(d => d.Price) })
                                .Take(10)
                                .ToList();

            return Ok(result);
        }
    }
}