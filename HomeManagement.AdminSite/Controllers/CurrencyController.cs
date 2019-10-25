using HomeManagement.AdminSite.Filters;
using HomeManagement.AdminSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            this.currencyService = currencyService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currencies = await currencyService.GetCurrencies();
            return View(currencies);
        }
    }
}