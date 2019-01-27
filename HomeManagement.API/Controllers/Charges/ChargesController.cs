using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace HomeManagement.API.Controllers.Charges
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Charges")]
    public class ChargesController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly Data.Repositories.IChargeRepository chargeRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IChargeMapper chargeMapper;
        private readonly ICategoryMapper categoryMapper;

        public ChargesController(IAccountRepository accountRepository,
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

        [HttpGet]
        public IActionResult Get()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

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

                chargeRepository.Add(entity, true);
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

            var entity = chargeMapper.ToEntity(model);

            chargeRepository.Update(entity, true);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id < 1) return BadRequest();

            chargeRepository.Remove(chargeRepository.GetById(id), true);

            return Ok();
        }
    }
}