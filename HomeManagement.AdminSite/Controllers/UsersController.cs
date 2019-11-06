using HomeManagement.AdminSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userService.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await userService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}