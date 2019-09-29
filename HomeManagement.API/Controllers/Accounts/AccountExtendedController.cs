using HomeManagement.API.Controllers.Transactions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
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
    public class AccountExtendedController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly Data.Repositories.ITransactionRepository transactionRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;
        private readonly ITransactionMapper transactionMapper;
        private readonly ICategoryMapper categoryMapper;
        private readonly ICategoryRepository categoryRepository;

        public AccountExtendedController(IAccountRepository accountRepository,
            Data.Repositories.ITransactionRepository transactionRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository,
            ITransactionMapper transactionMapper,
            ICategoryMapper categoryMapper,
            ICategoryRepository categoryRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
            this.transactionMapper = transactionMapper;
            this.categoryMapper = categoryMapper;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost("paging")]
        public IActionResult Page([FromBody]AccountPageModel model)
        {
            if (model == null) return BadRequest();

            if (model.TotalPages.Equals(default(int)))
            {
                var total = (double)accountRepository.All.Count(c => c.UserId.Equals(model.UserId));
                var totalPages = System.Math.Ceiling(total / (double)model.PageCount);
                model.TotalPages = int.Parse(totalPages.ToString());
            }

            var currentPage = model.CurrentPage - 1;

            model.Accounts = accountRepository
                .All
                .Where(x => x.UserId.Equals(model.UserId))
                .OrderByDescending(x => x.Id)
                .Skip(model.PageCount * currentPage)
                .Take(model.PageCount)
                .Select(x => accountMapper.ToModel(x))
                .ToList();

            return Ok(model);
        }

        [HttpPost("transfer")]
        public IActionResult Post([FromBody]TransferDto model)
        {
            if (model == null) return BadRequest();

            var controller = new TransactionsController(accountRepository, transactionRepository, categoryRepository, transactionMapper, categoryMapper, userRepository);

            var incomeTransaction = new TransactionModel
            {
                Name = model.OperationName,
                Price = model.Price,
                Date = DateTime.Now,
                TransactionType = TransactionTypeModel.Income,
                AccountId = model.TargetAccountId,
                CategoryId = model.CategoryId
            };

            controller.Post(incomeTransaction);

            var outcomeTransaction = new TransactionModel
            {
                Name = model.OperationName,
                Price = model.Price,
                Date = DateTime.Now,
                TransactionType = TransactionTypeModel.Expense,
                AccountId = model.SourceAccountId,
                CategoryId = model.CategoryId
            };

            controller.Post(outcomeTransaction);

            return Ok();
        }
    }
}