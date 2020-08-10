using HomeManagement.AdminSite.Services;
using HomeManagement.Models;
using HomeManagement.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IActionResult> Delete(string id)
        {
            if (id.IsEmpty()) return BadRequest();

            var users = await GetUsers();
            var userToDelete = users.First(x => x.Id.Equals(id));
            await userService.Delete(userToDelete.Email);
            FlushUserModels();
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<UserIdentityModel>> GetUsers()
        {
            var users = cache.Get<IEnumerable<UserIdentityModel>>(nameof(GetUsers));

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