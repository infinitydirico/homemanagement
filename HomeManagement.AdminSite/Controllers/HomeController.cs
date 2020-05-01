using HomeManagement.AdminSite.Filters;
using HomeManagement.AdminSite.Models;
using HomeManagement.AdminSite.Services;
using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Threading.Tasks;
using HomeManagement.AdminSite.Views;

namespace HomeManagement.AdminSite.Controllers
{
    public class HomeController : Controller
    {
        private IMemoryCache _cache;
        private IAuthenticationService apiService;
        private IHttpContextAccessor httpContextAccessor;

        public HomeController(IMemoryCache memoryCache, 
            IAuthenticationService apiService,
            IHttpContextAccessor httpContextAccessor)
        {
            _cache = memoryCache;
            this.apiService = apiService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel userModel)
        {
            if(ModelState.IsValid)
            {
                var result = await apiService.Login(userModel);
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                _cache.CreateEntry(ip);
                _cache.Set(ip, result);
            }
            else
            {
                return RedirectToAction("Error");
            }

            var host = HttpContext.GetHost();
            var scheme = HttpContext.GetScheme();
            return Redirect($"{scheme}://{host}/Home/Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
