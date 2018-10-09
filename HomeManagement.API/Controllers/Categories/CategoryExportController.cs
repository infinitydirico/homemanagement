using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeManagement.API.Exportation;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Categories
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Category")]
    public class CategoryExportController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IChargeRepository chargeRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IChargeMapper chargeMapper;
        private readonly ICategoryMapper categoryMapper;
        private readonly IUserCategoryRepository userCategoryRepository;
        private readonly IExportableCategory exportableCategory;

        public CategoryExportController(IAccountRepository accountRepository,
                                    IChargeRepository chargeRepository,
                                    ICategoryRepository categoryRepository,
                                    IChargeMapper chargeMapper,
                                    ICategoryMapper categoryMapper,
                                    IUserRepository userRepository,
                                    IUserCategoryRepository userCategoryRepository,
                                    IExportableCategory exportableCategory)
        {
            this.accountRepository = accountRepository;
            this.chargeRepository = chargeRepository;
            this.categoryRepository = categoryRepository;
            this.chargeMapper = chargeMapper;
            this.categoryMapper = categoryMapper;
            this.userRepository = userRepository;
            this.userCategoryRepository = userCategoryRepository;
            this.exportableCategory = exportableCategory;
        }

        [HttpGet("download")]
        public IActionResult DownloadCategories()
        {
            var email = HttpContext.GetEmailClaim();

            var categories = (from category in categoryRepository.All
                              join userCategory in userCategoryRepository.All
                              on category.Id equals userCategory.CategoryId
                              join user in userRepository.All
                              on userCategory.UserId equals user.Id
                              where user.Email.Equals(email.Value)
                              select category).ToList();

            var csv = exportableCategory.ToCsv(categories);

            var filename = "categories_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";

            HttpContext.Response.ContentType = "text/csv";

            HttpContext.Response.Headers.Add("Filename", filename);

            FileContentResult result = new FileContentResult(csv, "text/csv")
            {
                FileDownloadName = filename
            };

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

                var entities = exportableCategory.ToEntities(bytes);

                foreach (var entity in entities)
                {
                    if (entity == null) continue;

                    if (categoryRepository.Exists(entity)) continue;

                    entity.Id = 0;

                    categoryRepository.Add(entity, emailClaim);
                }
            }

            return Ok();
        }
    }
}