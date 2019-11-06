using HomeManagement.AdminSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    public class UserCategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public UserCategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var userCategories = await categoryService.GetUserCategories();

            return View(userCategories);
        }
    }
}