using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Charges
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Charges")]
    public class ChargesController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IChargeRepository chargeRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IChargeMapper chargeMapper;
        private readonly ICategoryMapper categoryMapper;

        public ChargesController(IAccountRepository accountRepository,
            IChargeRepository chargeRepository,
            ICategoryRepository categoryRepository,
            IChargeMapper chargeMapper,
            ICategoryMapper categoryMapper) 
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.categoryRepository = categoryRepository;
            this.chargeMapper = chargeMapper;
            this.categoryMapper = categoryMapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var claim = HttpContext
             .GetAuthorizationHeader()
             .GetJwtSecurityToken()
             .Claims
             .FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            if (claim == null) return BadRequest();

            var charges = (from charge in chargeRepository.All
                           join account in accountRepository.All
                           on charge.AccountId equals account.Id
                           join user in userRepository.All
                           on account.UserId equals user.Id
                           where user.Email.Equals(claim.Value)
                           select chargeMapper.ToModel(charge)).ToList();

            return Ok(charges);

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = chargeRepository.GetById(id);

            return Ok(chargeMapper.ToModel(result));
        }

        [HttpPost]
        public IActionResult Post([FromBody] ChargeModel model)
        {
            Category category;
            if (model == null) return BadRequest();

            if (model.CategoryId.Equals(default(int)) || categoryRepository.All.FirstOrDefault(x => x.Id.Equals(model.CategoryId)) == null)
            {
                category = categoryRepository.FirstOrDefault();//categoryRepository.GetDefaultCategory();
                model.CategoryId = category.Id;
            }

            try
            {
                var entity = chargeMapper.ToEntity(model);

                chargeRepository.Add(entity);

                UpdateBalance(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]ChargeModel model)
        {
            if (model == null) return BadRequest();

            var previousChargeValue = chargeRepository.GetById(model.Id);

            var entity = chargeMapper.ToEntity(model);

            chargeRepository.Update(entity);

            UpdateBalance(previousChargeValue, true);

            UpdateBalance(entity);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id < 1) return BadRequest();

            UpdateBalance(chargeRepository.GetById(id), true);

            chargeRepository.Remove(id);

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