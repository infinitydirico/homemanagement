using HomeManagement.API.Business;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeManagement.API.Controllers.Transactions
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Transactions")]
    public class TransactionsExportController : Controller
    {
        private readonly ITransactionService transactionService;

        public TransactionsExportController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet("download/{accountId}")]
        public IActionResult DownloadCategories(int accountId)
        {
            var fileResult = transactionService.Export(accountId);

            var result = this.CreateCsvFile(fileResult.Content, fileResult.Filename);

            return result;
        }

        [HttpPost("upload/{accountId}")]
        public IActionResult UploadCategories(int accountId)
        {
            var basePath = AppContext.BaseDirectory + "\\{0}";
            if (Request.Form == null) return BadRequest();

            foreach (IFormFile formFile in Request.Form.Files)
            {
                transactionService.Import(accountId, formFile);
            }

            return Ok();
        }
    }
}