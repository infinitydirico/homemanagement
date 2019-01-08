using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.FilesStore;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Global
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Storage")]
    public class StorageController : Controller
    {
        private readonly IStorageItemMapper storageItemMapper;
        private readonly IStorageItemRepository storageItemRepository;
        private readonly IStorageClient storageClient;
        private readonly IUserRepository userRepository;
        private readonly IChargeRepository chargeRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IPreferencesRepository preferencesRepository;

        public StorageController(IStorageItemMapper storageItemMapper,
            IStorageItemRepository storageItemRepository,
            IStorageClient storageClient,
            IUserRepository userRepository,
            IChargeRepository chargeRepository,
            IAccountRepository accountRepository,
            IPreferencesRepository preferencesRepository)
        {
            this.storageItemMapper = storageItemMapper;
            this.storageItemRepository = storageItemRepository;
            this.storageClient = storageClient;
            this.userRepository = userRepository;
            this.chargeRepository = chargeRepository;
            this.accountRepository = accountRepository;
            this.preferencesRepository = preferencesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = userRepository.GetByEmail(HttpContext.GetEmailClaim().Value);

            if (!storageClient.IsAuthorized(user.Id)) return Forbid();

            var clientFiles = await storageClient.Get(user.Id);

            var storageItems = (from storageItem in storageItemRepository.All
                                join charge in chargeRepository.All
                                on storageItem.ChargeId equals charge.Id
                                join account in accountRepository.All
                                on charge.AccountId equals account.Id
                                where account.UserId.Equals(user.Id)
                                select storageItem).ToList();

            if (clientFiles.All(x => storageItems.Exists(s => s.Name.Equals(x.Name))))
            {
                return Ok(storageItems);
            }

            //try to create missing files on repo

            return Ok();
        }

        [HttpGet("connect")]
        public IActionResult Connect()
        {
            var user = userRepository.GetByEmail(HttpContext.GetEmailClaim().Value);

            var accessToken = storageClient.GetAccessToken(user.Id);

            return Ok(accessToken.ToString());
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize([FromQuery(Name = "state")]string state, [FromQuery(Name = "code")]string code)
        {
            var preference = preferencesRepository.FirstOrDefault(x => x.Value.Equals(state));

            var user = userRepository.GetById(preference.UserId);

            await storageClient.Authorize(user.Id, code, state);

            return Ok();
        }

        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var claim = HttpContext.GetEmailClaim();

            var user = userRepository.GetByEmail(claim.Value);

            var filename = HttpContext.GetHeader("filename");

            await storageClient.Upload(user.Id, filename, Request.Body);

            return Ok();
        }
    }
}