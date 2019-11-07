using HomeManagement.AdminSite.Services;
using HomeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IMemoryCache cache;

        public UsersController(IUserService userService, IMemoryCache cache)
        {
            this.userService = userService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetUsers());
        }

        public async Task<IActionResult> Delete(int id)
        {
            await userService.Delete(id);
            FlushUserModels();
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<UserModel>> GetUsers()
        {
            var users = cache.Get<IEnumerable<UserModel>>(nameof(GetUsers));

            if (users != null) return users;

            users = await userService.GetUsers();

            cache.CreateEntry(nameof(GetUsers));
            cache.Set(nameof(GetUsers), users, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)
            });

            return users;
        }

        private void FlushUserModels()
        {
            cache.Remove(nameof(GetUsers));
        }
    }
}