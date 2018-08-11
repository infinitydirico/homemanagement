using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("swagger");
        }
    }
}