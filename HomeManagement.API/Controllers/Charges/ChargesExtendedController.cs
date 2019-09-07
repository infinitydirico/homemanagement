using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HomeManagement.API.Controllers.Charges
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Charges")]
    public class ChargesExtendedController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly Data.Repositories.IChargeRepository chargeRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IChargeMapper chargeMapper;
        private readonly ICategoryMapper categoryMapper;

        public ChargesExtendedController(IAccountRepository accountRepository,
            Data.Repositories.IChargeRepository chargeRepository,
            ICategoryRepository categoryRepository,
            IChargeMapper chargeMapper,
            ICategoryMapper categoryMapper,
            IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.categoryRepository = categoryRepository;
            this.chargeMapper = chargeMapper;
            this.categoryMapper = categoryMapper;
            this.userRepository = userRepository;
        }

        [HttpPost("paging")]
        public IActionResult Page([FromBody]ChargePageModel model)
        {
            if (model == null) return BadRequest();

            Expression<Func<Charge, bool>> predicate = o => o.AccountId.Equals(model.AccountId);
            Func<Charge, bool> filter = predicate.Compile();

            var total = (double)chargeRepository.Count(predicate);

            var totalPages = Math.Ceiling(total / (double)model.PageCount);

            model.TotalPages = int.Parse(totalPages.ToString());

            bool isFiltering = !string.IsNullOrEmpty(model.Property) && !string.IsNullOrEmpty(model.FilterValue);

            if (isFiltering)
            {
                filter = filter.Where(model.Property, model.FilterValue.ToLower(), model.Operator.IntToOperator());
            }

            var currentPage = model.CurrentPage - 1;

            model.Charges = chargeRepository
                            .All
                            .Where(filter)
                            .OrderByDescending(x => x.Id)
                            .Skip(model.PageCount * currentPage)
                            .Take(model.PageCount)
                            .Select(x => chargeMapper.ToModel(x))
                            .ToList();

            return Ok(model);
        }

        [HttpGet("by/date/{year}/{month}")]
        public IActionResult ByDate(int year, int month)
        {
            var claim = HttpContext.GetEmailClaim();

            var charges = (from user in userRepository.All
                           join account in accountRepository.All
                           on user.Id equals account.UserId
                           join charge in chargeRepository.All
                           on account.Id equals charge.AccountId
                           where user.Email.Equals(claim.Value) &&
                                    charge.Date.Year.Equals(year) &&
                                    charge.Date.Month.Equals(month)
                           orderby charge.Date descending
                           select chargeMapper.ToModel(charge))
                            .ToList();

            return Ok(charges);
        }

        [HttpGet("by/date/{year}/{month}/account/{accountId}")]
        public IActionResult ByDateAndAccount(int year, int month, int accountId)
        {
            var claim = HttpContext.GetEmailClaim();

            var charges = (from user in userRepository.All
                           join account in accountRepository.All
                           on user.Id equals account.UserId
                           join charge in chargeRepository.All
                           on account.Id equals charge.AccountId
                           where user.Email.Equals(claim.Value) &&
                                    account.Id.Equals(accountId) &&
                                    charge.Date.Year.Equals(year) &&
                                    charge.Date.Month.Equals(month)
                           orderby charge.Date descending
                           select chargeMapper.ToModel(charge))
                            .ToList();

            return Ok(charges);
        }

        [HttpGet("by/category/{category}")]
        public IActionResult ByCategory(int category)
        {
            var claim = HttpContext.GetEmailClaim();

            var charges = (from user in userRepository.All
                           join account in accountRepository.All
                           on user.Id equals account.UserId
                           join charge in chargeRepository.All
                           on account.Id equals charge.AccountId
                           where user.Email.Equals(claim.Value) &&
                                    charge.Date.Year.Equals(DateTime.Now.Year) &&
                                    charge.Date < DateTime.Now &&
                                    charge.CategoryId.Equals(category)
                           orderby charge.Date ascending
                           select chargeMapper.ToModel(charge))
                           .GroupBy(x => x.Date.Month)
                           .Select(x => new
                           {
                               Month = x.First().Date.ToString("MMMM"),
                               Price = x.Sum(z => z.Price)
                           })
                            .ToList();

            return Ok(charges);
        }

        [HttpGet("by/account/{accountId}/category/{category}")]
        public IActionResult ByAccountAndCategory(int accountId, int category)
        {
            var claim = HttpContext.GetEmailClaim();

            var charges = (from user in userRepository.All
                           join account in accountRepository.All
                           on user.Id equals account.UserId
                           join charge in chargeRepository.All
                           on account.Id equals charge.AccountId
                           where user.Email.Equals(claim.Value) &&
                                    charge.AccountId.Equals(accountId) &&
                                    charge.Date.Year.Equals(DateTime.Now.Year) &&
                                    charge.Date < DateTime.Now &&
                                    charge.CategoryId.Equals(category)
                           orderby charge.Date ascending
                           select chargeMapper.ToModel(charge))
                           .GroupBy(x => x.Date.Month)
                           .Select(x => new
                           {
                               Month = x.First().Date.ToString("MMMM"),
                               Price = x.Sum(z => z.Price)
                           })
                            .ToList();

            return Ok(charges);
        }

        [HttpGet("getlastfive")]
        public IActionResult GetLastFive()
        {
            var claim = HttpContext.GetEmailClaim();

            var charges = (from user in userRepository.All
                           join account in accountRepository.All
                           on user.Id equals account.UserId
                           join charge in chargeRepository.All
                           on account.Id equals charge.AccountId
                           where user.Email.Equals(claim.Value)
                           orderby charge.Date descending
                           select chargeMapper.ToModel(charge))
                            .Take(5)
                            .ToList();

            return Ok(charges);
        }

        [HttpPost("updateAll")]
        public IActionResult UpdateAll([FromBody] IEnumerable<ChargeModel> models)
        {
            if (models == null || models.Count().Equals(default(int))) return BadRequest();

            foreach (var charge in models)
            {
                chargeRepository.Update(chargeMapper.ToEntity(charge), true);
            }

            return Ok();
        }

        [HttpDelete("deleteall/{accountId}")]
        public IActionResult DeleteAll(int accountId)
        {
            if (accountId < 1) return BadRequest();

            var account = accountRepository.FirstOrDefault(x => x.Id.Equals(accountId));

            var charges = chargeRepository
                .All
                .Where(o => o.AccountId.Equals(accountId))
                .ToList();

            foreach (var charge in charges)
            {
                chargeRepository.Remove(charge);

                account.Balance = charge.ChargeType.Equals(ChargeType.Income) ? account.Balance - charge.Price : account.Balance + charge.Price; //it's a reverse.
                accountRepository.Update(account);
            }
            chargeRepository.Commit();

            return Ok();
        }
    }
}