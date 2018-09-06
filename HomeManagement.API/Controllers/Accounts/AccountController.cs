using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        public AccountController(IAccountRepository accountRepository,
            IChargeRepository chargeRepository,
            IAccountMapper accountMapper)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.accountMapper = accountMapper;
        }       

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = accountRepository.GetById(id);

            if (account == null) return NotFound();

            return Ok(account);
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
            if (charge != null) return BadRequest("la cuenta tiene movimientos asociados");

            accountRepository.Remove(id);
            return Ok();
        }

        //[HttpPost("transfer")]
        //public IActionResult Post([FromBody]TransferDto model)
        //{
        //    if (model == null) return BadRequest();

        //    var controller = new ChargeController(accountRepository, chargeRepository, categoryRepository, taxRepository, userManager);

        //    var incomingCharge = new Charge
        //    {
        //        Name = model.OperationName,
        //        Price = model.Price,
        //        Date = DateTime.Now,
        //        ChargeType = ChargeType.Incoming,
        //        AccountId = model.TargetAccountId,
        //        CategoryId = model.CategoryId
        //    };

        //    controller.Post(incomingCharge);

        //    var outgoingCharge = new Charge
        //    {
        //        Name = model.OperationName,
        //        Price = model.Price,
        //        Date = DateTime.Now,
        //        ChargeType = ChargeType.Outgoing,
        //        AccountId = model.SourceAccountId,
        //        CategoryId = model.CategoryId
        //    };

        //    controller.Post(outgoingCharge);

        //    return Ok();
        //}
    }
}