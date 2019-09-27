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

namespace HomeManagement.API.Controllers.Transactions
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Charges")]
    [Persistable]
    public class TransactionsController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly Data.Repositories.TransactionRepository transactionRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionMapper transactionMapper;
        private readonly ICategoryMapper categoryMapper;

        public TransactionsController(IAccountRepository accountRepository,
            Data.Repositories.TransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            ITransactionMapper transactionMapper,
            ICategoryMapper categoryMapper,
            IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.transactionMapper = transactionMapper;
            this.categoryMapper = categoryMapper;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            if (claim == null) return BadRequest();

            var transactions = (from charge in transactionRepository.All
                           join account in accountRepository.All
                           on charge.AccountId equals account.Id
                           join user in userRepository.All
                           on account.UserId equals user.Id
                           where user.Email.Equals(claim.Value)
                           select transactionMapper.ToModel(charge)).ToList();

            return Ok(transactions);

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = transactionRepository.GetById(id);

            return Ok(transactionMapper.ToModel(result));
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransactionModel model)
        {
            Category category;
            if (model == null) return BadRequest();

            if (model.CategoryId.Equals(default(int)) || categoryRepository.All.FirstOrDefault(x => x.Id.Equals(model.CategoryId)) == null)
            {
                category = categoryRepository.FirstOrDefault();
                model.CategoryId = category.Id;
            }

            var entity = transactionMapper.ToEntity(model);

            transactionRepository.Add(entity);
            UpdateBalance(entity);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]TransactionModel model)
        {
            if (model == null) return BadRequest();

            var entity = transactionMapper.ToEntity(model);

            transactionRepository.Update(entity, true);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id < 1) return BadRequest();

            transactionRepository.Remove(transactionRepository.GetById(id), true);

            return Ok();
        }

        private void UpdateBalance(Transaction c, bool reverse = false)
        {
            var account = accountRepository.FirstOrDefault(x => x.Id.Equals(c.AccountId));

            if (reverse)
            {
                c.Price = -c.Price;
            }

            account.Balance = c.TransactionType.Equals(TransactionType.Income) ? account.Balance + c.Price : account.Balance - c.Price;
            accountRepository.Update(account);
        }
    }
}