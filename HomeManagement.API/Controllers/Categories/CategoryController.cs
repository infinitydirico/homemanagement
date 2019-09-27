using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Controllers.Categories
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Category")]
    [Persistable]
    public class CategoryController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly Data.Repositories.ITransactionRepository transactionRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionMapper chargeMapper;
        private readonly ICategoryMapper categoryMapper;
        private readonly IUserCategoryRepository userCategoryRepository;

        public CategoryController(IAccountRepository accountRepository,
                                    Data.Repositories.ITransactionRepository transactionRepository,
                                    ICategoryRepository categoryRepository,
                                    ITransactionMapper chargeMapper,
                                    ICategoryMapper categoryMapper,
                                    IUserRepository userRepository,
                                    IUserCategoryRepository userCategoryRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.chargeMapper = chargeMapper;
            this.categoryMapper = categoryMapper;
            this.userRepository = userRepository;
            this.userCategoryRepository = userCategoryRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var email = HttpContext.GetEmailClaim();

            var categories = (from category in categoryRepository.All
                              join userCategory in userCategoryRepository.All
                              on category.Id equals userCategory.CategoryId
                              join user in userRepository.All
                              on userCategory.UserId equals user.Id
                              where user.Email.Equals(email.Value)
                              select categoryMapper.ToModel(category)).ToList();

            return Ok(categories);
        }

        [HttpGet("active")]
        public IActionResult GetActiveCategories()
        {
            var email = HttpContext.GetEmailClaim();

            var categories = (from category in categoryRepository.All
                              join userCategory in userCategoryRepository.All
                              on category.Id equals userCategory.CategoryId
                              join user in userRepository.All
                              on userCategory.UserId equals user.Id
                              where user.Email.Equals(email.Value) && category.IsActive
                              select categoryMapper.ToModel(category)).ToList();

            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Post([FromBody]CategoryModel category)
        {
            if (category == null) return BadRequest();

            categoryRepository.Add(categoryMapper.ToEntity(category), userRepository.FirstOrDefault(x => x.Email.Equals(HttpContext.GetEmailClaim().Value)));
            categoryRepository.Commit();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]CategoryModel category)
        {
            if (category == null) return BadRequest();

            if (!(category.Id > 0)) return BadRequest();

            categoryRepository.Update(categoryMapper.ToEntity(category));
            categoryRepository.Commit();

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var category = categoryRepository.GetById(id);

            if (category == null) return BadRequest();

            var chargesWithThisCategory = transactionRepository.Count(x => x.CategoryId.Equals(id));

            if (chargesWithThisCategory > default(int)) return BadRequest();

            categoryRepository.Remove(id, userRepository.FirstOrDefault(x => x.Email.Equals(HttpContext.GetEmailClaim().Value)));
            categoryRepository.Commit();

            return Ok();
        }
    }
}