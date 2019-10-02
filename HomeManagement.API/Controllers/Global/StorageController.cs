using HomeManagement.API.Business;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.API.Controllers.Global
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Storage")]
    public class StorageController : Controller
    {
        private readonly IStorageService storageService;

        public StorageController(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        [HttpGet]
        [StorageAuthorization]
        public IActionResult Get()
        {
            //try to create missing files on repo

            return Ok(storageService.GetStorageItems(HttpContext.GetEmailClaim().Value));
        }

        [HttpGet("getitems")]
        [StorageAuthorization]
        public IActionResult GetItems()
        {
            return Ok(storageService.GetStorageItems(HttpContext.GetEmailClaim().Value));
        }
        
        [HttpGet("gettransactionfiles/{transactionId}")]
        [StorageAuthorization]
        public IActionResult GetTransactionFiles(int transactionId)
        {
            return Ok(storageService.GetTransactionFiles(HttpContext.GetEmailClaim().Value, transactionId));
        }

        [HttpGet("connect")]
        public IActionResult Connect()
        {
            return Ok(storageService.CreateAccessToken(HttpContext.GetEmailClaim().Value));
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize([FromQuery(Name = "state")]string state, [FromQuery(Name = "code")]string code)
        {
            var result = await storageService.Authorize(state, code);

            return Ok();
        }

        [HttpGet("download/{id}")]
        [StorageAuthorization]
        public async Task<IActionResult> Download(int id)
        {
            var file = await storageService.Download(id, HttpContext.GetEmailClaim().Value);
            return new FileStreamResult(file.Stream, file.ContentType);
        }

        [HttpPost("upload")]
        [StorageAuthorization]
        public async Task<IActionResult> Upload()
        {
            var claim = HttpContext.GetEmailClaim();

            var filename = HttpContext.GetHeader("filename");
            var transactionId = int.Parse(HttpContext.GetHeader("transactionId"));

            var storageItem = await storageService.Upload(filename, transactionId, Request.Body);

            return Ok(storageItem);
        }
    }
}