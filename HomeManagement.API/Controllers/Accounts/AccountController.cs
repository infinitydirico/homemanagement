using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Collections.Generic;
using HomeManagement.Core.Common;

namespace HomeManagement.API.Controllers.Accounts
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IChargeRepository chargeRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;

        public AccountController(IAccountRepository accountRepository,
            IChargeRepository chargeRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
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

            var accounts = (from account in accountRepository.All
                            join user in userRepository.All
                            on account.UserId equals user.Id
                            where user.Email.Equals(claim.Value)
                            select accountMapper.ToModel(account))
                            .ToList();

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = accountRepository.GetById(id);

            if (account == null) return NotFound();

            return Ok(accountMapper.ToModel(account));
        }

        [HttpPost]
        public IActionResult Post([FromBody]AccountModel model)
        {
            if (model == null && !ModelState.IsValid) return BadRequest();

            var entity = accountMapper.ToEntity(model);

            accountRepository.Add(entity);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]AccountModel model)
        {
            if (model == null && !ModelState.IsValid) return BadRequest();

            var entity = accountMapper.ToEntity(model);

            accountRepository.Update(entity);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id < 1) return BadRequest();

            var charge = this.chargeRepository.FirstOrDefault(c => c.AccountId.Equals(id));
            if (charge != null) return BadRequest(Constants.ErrorCode.AccountHasCharges);

            accountRepository.Remove(id);

            return Ok();
        }
    }
}