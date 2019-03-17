using System;
using System.Collections.Generic;
using System.Linq;
using HomeManagement.API.Exportation;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Charges
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Charges")]
    public class ChargesExportController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly Data.Repositories.IChargeRepository chargeRepository;
        private readonly IUserRepository userRepository;
        private readonly IExportableCharge exportableCharge;

        public ChargesExportController(IAccountRepository accountRepository,
            Data.Repositories.IChargeRepository chargeRepository,
            ICategoryRepository categoryRepository,
            IChargeMapper chargeMapper,
            ICategoryMapper categoryMapper,
            IUserRepository userRepository,
            IExportableCharge exportableCharge)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.userRepository = userRepository;
            this.exportableCharge = exportableCharge;
        }

        [HttpGet("download")]
        public IActionResult DownloadCategories()
        {
            var email = HttpContext.GetEmailClaim();

            var charges = (from charge in chargeRepository.All
                              join account in accountRepository.All
                              on charge.AccountId equals account.Id
                              join user in userRepository.All
                              on account.UserId equals user.Id
                              where user.Email.Equals(email.Value)
                              select charge).ToList();

            var csv = exportableCharge.ToCsv(charges);

            var filename = "charges_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";

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

                    account.Balance = entity.ChargeType.Equals(ChargeType.Income) ? account.Balance + entity.Price : account.Balance - entity.Price;
                    accountRepository.Update(account);
                }
                chargeRepository.Commit();
            }

            return Ok();
        }

        private void UpdateBalance(Charge c, bool reverse = false)
        {
            var account = accountRepository.FirstOrDefault(x => x.Id.Equals(c.AccountId));

            if (reverse)
            {
                c.Price = -c.Price;
            }

            account.Balance = c.ChargeType.Equals(ChargeType.Income) ? account.Balance + c.Price : account.Balance - c.Price;
            accountRepository.Update(account);
        }
    }
}