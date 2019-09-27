using HomeManagement.API.Exportation;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HomeManagement.API.Controllers.Transactions
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Charges")]
    [Persistable]
    public class TransactionsExportController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly Data.Repositories.TransactionRepository chargeRepository;
        private readonly IUserRepository userRepository;
        private readonly IExportableCharge exportableCharge;

        public TransactionsExportController(IAccountRepository accountRepository,
            Data.Repositories.TransactionRepository chargeRepository,
            ICategoryRepository categoryRepository,
            ITransactionMapper chargeMapper,
            ICategoryMapper categoryMapper,
            IUserRepository userRepository,
            IExportableCharge exportableCharge)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.userRepository = userRepository;
            this.exportableCharge = exportableCharge;
        }

        [HttpGet("download/{accountId}")]
        public IActionResult DownloadCategories(int accountId)
        {
            var charges = chargeRepository.Where(x => x.AccountId.Equals(accountId)).ToList();

            var account = accountRepository.GetById(accountId);

            var csv = exportableCharge.ToCsv(charges);

            var filename = $"{account}{DateTime.Now.ToString("yyyyMMddhhmmss")}.csv";

            var result = this.CreateCsvFile(csv, filename);

            return result;
        }

        [HttpPost("upload/{accountId}")]
        public IActionResult UploadCategories(int accountId)
        {
            var basePath = AppContext.BaseDirectory + "\\{0}";
            if (Request.Form == null) return BadRequest();

            var emailClaim = userRepository.FirstOrDefault(x => x.Email.Equals(HttpContext.GetEmailClaim().Value));

            foreach (IFormFile formFile in Request.Form.Files)
            {
                var account = accountRepository.FirstOrDefault(x => x.Id.Equals(accountId));

                foreach (var entity in exportableCharge.ToEntities(formFile.OpenReadStream().GetBytes()))
                {
                    if (entity == null) continue;

                    if (chargeRepository.Exists(entity)) continue;

                    entity.Id = 0;
                    entity.AccountId = accountId;
                    chargeRepository.Add(entity);

                    account.Balance = entity.TransactionType.Equals(TransactionType.Income) ? account.Balance + entity.Price : account.Balance - entity.Price;
                    accountRepository.Update(account);
                }
            }

            return Ok();
        }
    }
}