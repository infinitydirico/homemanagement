using HomeManagement.API.Extensions;
using HomeManagement.Core.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Business.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeManagement.API.Controllers.Categories
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Category")]
    public class CategoryExportController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryExportController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("download")]
        public IActionResult DownloadCategories()
        {
            var file = categoryService.Export();

            var result = this.CreateCsvFile(file.Contents, file.Name);

            return result;
        }

        [HttpPost("upload")]
        public IActionResult UploadCategories()
        {
            var basePath = AppContext.BaseDirectory + "\\{0}";
            if (Request.Form == null) return BadRequest();

            foreach (IFormFile formFile in Request.Form.Files)
            {
                var bytes = formFile.OpenReadStream().GetBytes();

                categoryService.Import(bytes);
            }

            return Ok();
        }
    }
}