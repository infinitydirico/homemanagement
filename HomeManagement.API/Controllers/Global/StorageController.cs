using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.FilesStore;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.API.Controllers.Global
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Storage")]
    [Persistable]
    public class StorageController : Controller
    {
        private readonly IStorageItemMapper storageItemMapper;
        private readonly IStorageItemRepository storageItemRepository;
        private readonly IStorageClient storageClient;
        private readonly IUserRepository userRepository;
        private readonly Data.Repositories.ITransactionRepository transactionRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IPreferencesRepository preferencesRepository;

        public StorageController(IStorageItemMapper storageItemMapper,
            IStorageItemRepository storageItemRepository,
            IStorageClient storageClient,
            IUserRepository userRepository,
            Data.Repositories.ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IPreferencesRepository preferencesRepository)
        {
            this.storageItemMapper = storageItemMapper;
            this.storageItemRepository = storageItemRepository;
            this.storageClient = storageClient;
            this.userRepository = userRepository;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.preferencesRepository = preferencesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = userRepository.GetByEmail(HttpContext.GetEmailClaim().Value);

            if (!storageClient.IsAuthorized(user.Id)) return Forbid();

            var clientFiles = await storageClient.Get(user.Id);

            var storageItems = GetRepoItems(user.Id);

            if (clientFiles.All(x => storageItems.Exists(s => s.Name.Equals(x.Name))))
            {
                return Ok(storageItems.Select(x => storageItemMapper.ToModel(x)));
            }

            //try to create missing files on repo

            return Ok();
        }

        [HttpGet("getitems")]
        public IActionResult GetItems()
        {
            var user = userRepository.GetByEmail(HttpContext.GetEmailClaim().Value);

            if (!storageClient.IsAuthorized(user.Id)) return Forbid();

            return Ok(GetRepoItems(user.Id).Select(x => storageItemMapper.ToModel(x)));
        }
        
        [HttpGet("getchargefiles/{chargeId}")]
        public IActionResult GetChargeFles(int chargeId)
        {
            var user = userRepository.GetByEmail(HttpContext.GetEmailClaim().Value);

            if (!storageClient.IsAuthorized(user.Id)) return Forbid();

            return Ok(GetRepoItems(user.Id)
                .Where(x => x.TransactionId.Equals(chargeId))
                .Select(x => storageItemMapper.ToModel(x)));
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

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var claim = HttpContext.GetEmailClaim();

            var user = userRepository.GetByEmail(claim.Value);

            var item = storageItemRepository.GetById(id);

            var fileStream = await storageClient.Download(user.Id, item.Path);

            var file = new FileModel
            {
                Name = item.Name,
            };
            return new FileStreamResult(fileStream, file.ContentType);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            int chargeId = 0;
            Account account = null;
            Transaction charge = null;

            var claim = HttpContext.GetEmailClaim();

            var user = userRepository.GetByEmail(claim.Value);

            if (!storageClient.IsAuthorized(user.Id)) return Forbid();

            var filename = HttpContext.GetHeader("filename");

            if (HttpContext.HasHeader("chargeId"))
            {
                chargeId = int.Parse(HttpContext.GetHeader("chargeId"));
                charge = transactionRepository.GetById(chargeId);
                account = accountRepository.GetById(charge.AccountId);
            }

            var storageItem = await storageClient.Upload(user.Id, filename,account.Name, charge.Name, Request.Body);

            storageItem.TransactionId = chargeId;

            storageItemRepository.Add(storageItem);

            return Ok(storageItem);
        }

        private List<StorageItem> GetRepoItems(int userId)
        {
            return (from storageItem in storageItemRepository.All
                    join charge in transactionRepository.All
                    on storageItem.TransactionId equals charge.Id
                    join account in accountRepository.All
                    on charge.AccountId equals account.Id
                    where account.UserId.Equals(userId)
                    select storageItem).ToList();
        }
    }
}