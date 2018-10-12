using System;
using System.Collections.Generic;
using System.Linq;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using HomeManagement.Core.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.IdentityModel.Tokens.Jwt;
using HomeManagement.API.Extensions;

namespace HomeManagement.API.Controllers.Charges
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Charges")]
    public class ChargesExtendedController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IChargeRepository chargeRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IChargeMapper chargeMapper;
        private readonly ICategoryMapper categoryMapper;

        public ChargesExtendedController(IAccountRepository accountRepository,
            IChargeRepository chargeRepository,
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
                chargeRepository.Update(chargeMapper.ToEntity(charge));
            }

            return Ok();
        }

        [HttpDelete("deleteall/{accountId}")]
        public IActionResult DeleteAll(int accountId)
        {
            if (accountId < 1) return BadRequest();

            var charges = chargeRepository
                .All
                .Where(o => o.AccountId.Equals(accountId))
                .ToList();

            foreach (var charge in charges)
            {
                chargeRepository.Remove(charge.Id);
                UpdateBalance(charge, true);

            }
            return Ok();
        }


        private void UpdateBalance(Charge c, bool reverse = false)
        {
            var account = accountRepository.GetById(c.AccountId);

            if (reverse)
            {
                c.Price = -c.Price;
            }

            account.Balance = c.ChargeType.Equals(ChargeType.Income) ? account.Balance + c.Price : account.Balance - c.Price;
            accountRepository.Update(account);
        }
    }
}