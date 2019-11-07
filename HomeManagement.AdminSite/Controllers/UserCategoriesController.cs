using HomeManagement.AdminSite.Services;
using HomeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    public class UserCategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMemoryCache cache;

        public UserCategoriesController(ICategoryService categoryService, IMemoryCache cache)
        {
            this.categoryService = categoryService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var userCategories = await GetUserCategories();

            return View(userCategories);
        }

        private async Task<IEnumerable<UserCategoryModel>> GetUserCategories()
        {
            var userCategories = cache.Get<IEnumerable<UserCategoryModel>>(nameof(GetUserCategories));

            if (userCategories != null) return userCategories;

            userCategories = await categoryService.GetUserCategories();

            cache.CreateEntry(nameof(GetUserCategories));
            cache.Set(nameof(GetUserCategories), userCategories, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)
            });

            return userCategories;
        }

        private void FlushCategories()
        {
            cache.Remove(nameof(GetUserCategories));
        }
    }
}