using HomeManagement.API.Business;
using HomeManagement.Api.Core.Extensions;
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
            return Ok(storageService.GetStorageItems());
        }

        [HttpGet("isconfigured")]
        public IActionResult IsConfigured() => Ok(storageService.IsConfigured());

        [HttpGet("getitems")]
        [StorageAuthorization]
        public IActionResult GetItems()
        {
            return Ok(storageService.GetStorageItems());
        }
        
        [HttpGet("gettransactionfiles/{transactionId}")]
        [StorageAuthorization]
        public IActionResult GetTransactionFiles(int transactionId)
        {
            if (transactionId < 1) return BadRequest();

            return Ok(storageService.GetTransactionFiles(transactionId));
        }

        [HttpGet("connect")]
        public IActionResult Connect()
        {
            return Ok(storageService.CreateAccessToken());
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize([FromQuery(Name = "state")]string state, [FromQuery(Name = "code")]string code)
        {
            if (string.IsNullOrEmpty(state) || string.IsNullOrEmpty(code)) return BadRequest();

            var result = await storageService.Authorize(state, code);

            return Ok();
        }

        [HttpGet("download/{id}")]
        [StorageAuthorization]
        public async Task<IActionResult> Download(int id)
        {
            if (id < 1) return BadRequest();

            var file = await storageService.Download(id);
            return new FileStreamResult(file.Stream, file.ContentType);
        }

        [HttpPost("upload")]
        [StorageAuthorization]
        public async Task<IActionResult> Upload()
        {
            if(!(HttpContext.HasHeader("filename") && HttpContext.HasHeader("transactionId")))
            {
                return BadRequest();
            }

            var filename = HttpContext.GetHeader("filename");
            var transactionId = int.Parse(HttpContext.GetHeader("transactionId"));

            var storageItem = await storageService.Upload(filename, transactionId, Request.Body);

            return Ok(storageItem);
        }
    }
}