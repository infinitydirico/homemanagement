using System;
using System.Collections.Generic;
using System.Linq;
using HomeManagement.API.Exportation;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
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
        private readonly IChargeRepository chargeRepository;
        private readonly IUserRepository userRepository;
        private readonly IExportableCharge exportableCharge;

        public ChargesExportController(IAccountRepository accountRepository,
            IChargeRepository chargeRepository,
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

        [HttpPost("upload")]
        public IActionResult UploadCategories()
        {
            var basePath = AppContext.BaseDirectory + "\\{0}";
            if (Request.Form == null) return BadRequest();

            var emailClaim = userRepository.FirstOrDefault(x => x.Email.Equals(HttpContext.GetEmailClaim().Value));

            foreach (var f in Request.Form.Files)
            {
                var formFile = f as IFormFile;

                var stream = formFile.OpenReadStream();

                var bytes = stream.GetBytes();

                var entities = exportableCharge.ToEntities(bytes);

                foreach (var entity in entities)
                {
                    if (entity == null) continue;

                    if (chargeRepository.Exists(entity)) continue;

                    entity.Id = 0;

                    chargeRepository.Add(entity);
                }
            }

            return Ok();
        }
    }
}