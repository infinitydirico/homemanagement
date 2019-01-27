using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
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
        private readonly Data.Repositories.IChargeRepository chargeRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategoryMapper categoryMapper;

        public AccountChartsController(IAccountRepository accountRepository,
            Data.Repositories.IChargeRepository chargeRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            ICategoryMapper categoryMapper)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
            this.categoryMapper = categoryMapper;
        }


        [HttpGet("{id}/chartbychargetype")]
        public IActionResult ChartData(int id) => Ok(new AccountOverviewModel
        {
            TotalCharges = chargeRepository.Count(c => c.AccountId.Equals(id)),
            ExpneseCharges = chargeRepository.Count(c => c.AccountId.Equals(id) && c.ChargeType == ChargeType.Expense),
            IncomeCharges = chargeRepository.Count(c => c.AccountId.Equals(id) && c.ChargeType == (int)ChargeType.Income)
        });

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
                var accountEvoModel = new AccountBalanceModel();

                for (int i = 1; i <= DateTime.Now.Month; i++)
                {
                    var incomingCharges = chargeRepository
                                       .Sum(c => int.Parse(c.Price.ToString("F0")), c => c.AccountId.Equals(account.Id)
                                                    && c.ChargeType == (int)ChargeType.Income
                                                    && c.Date.Month.Equals(i));

                    var outgoingCharges = chargeRepository
                                        .Sum(c => int.Parse(c.Price.ToString("F0")), c => c.AccountId.Equals(account.Id)
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
                var incomingCharges = chargeRepository
                                        .Sum(c => int.Parse(c.Price.ToString()), c => c.AccountId.Equals(id)
                                                     && c.ChargeType == (int)ChargeType.Income
                                                     && c.Date.Month.Equals(i));

                var outgoingCharges = chargeRepository
                                        .Sum(c => decimal.Parse(c.Price.ToString()), c => c.AccountId.Equals(id)
                                                     && c.ChargeType == ChargeType.Expense
                                                     && c.Date.Month.Equals(i));

                model.IncomingSeries.Add(decimal.ToInt32(incomingCharges));
                model.OutgoingSeries.Add(decimal.ToInt32(outgoingCharges));
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

        [HttpGet("topcharges/{month}")]
        public IActionResult AccountTopCharges(int month)
        {
            var email = HttpContext.GetEmailClaim();

            if (month.Equals(default(int)))
            {
                month = DateTime.Now.Month;
            }

            //implement a method where it gets all charges of all accounts to the authenticated user that is grouped by categories
            var result = (from charge in chargeRepository.All
                          join account in accountRepository.All
                          on charge.AccountId equals account.Id
                          join user in userRepository.All
                          on account.UserId equals user.Id
                          join category in categoryRepository.All
                          on charge.CategoryId equals category.Id
                          where user.Email.Equals(email.Value)
                                   && charge.ChargeType.Equals(ChargeType.Expense)
                                   && charge.Date.Month.Equals(month)
                          select new { Charge = charge, Category = category })
                         .Take(10)
                         .GroupBy(x => x.Category.Id)
                         .Select(x => new OverPricedCategory
                         {
                             Category = categoryMapper.ToModel(x.FirstOrDefault().Category),
                             Price = x.Sum(c => c.Charge.Price)
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