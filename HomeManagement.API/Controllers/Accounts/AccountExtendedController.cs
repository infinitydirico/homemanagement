using HomeManagement.API.Controllers.Charges;
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
        private readonly Data.Repositories.IChargeRepository chargeRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;
        private readonly IChargeMapper chargeMapper;
        private readonly ICategoryMapper categoryMapper;
        private readonly ICategoryRepository categoryRepository;

        public AccountExtendedController(IAccountRepository accountRepository,
            Data.Repositories.IChargeRepository chargeRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository,
            IChargeMapper chargeMapper,
            ICategoryMapper categoryMapper,
            ICategoryRepository categoryRepository)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
            this.chargeMapper = chargeMapper;
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

            var controller = new ChargesController(accountRepository, chargeRepository, categoryRepository, chargeMapper, categoryMapper, userRepository);

            var incomingCharge = new ChargeModel
            {
                Name = model.OperationName,
                Price = model.Price,
                Date = DateTime.Now,
                ChargeType = ChargeTypeModel.Income,
                AccountId = model.TargetAccountId,
                CategoryId = model.CategoryId
            };

            controller.Post(incomingCharge);

            var outgoingCharge = new ChargeModel
            {
                Name = model.OperationName,
                Price = model.Price,
                Date = DateTime.Now,
                ChargeType = ChargeTypeModel.Expense,
                AccountId = model.SourceAccountId,
                CategoryId = model.CategoryId
            };

            controller.Post(outgoingCharge);

            return Ok();
        }
    }
}