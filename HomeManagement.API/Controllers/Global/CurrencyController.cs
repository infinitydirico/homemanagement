using HomeManagement.API.Filters;
using HomeManagement.API.Services;
using HomeManagement.Mapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HomeManagement.API.Controllers.Global
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Currency")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyMapper currencyMapper;

        public CurrencyController(ICurrencyService currencyService, ICurrencyMapper currencyMapper)
        {
            this.currencyService = currencyService;
            this.currencyMapper = currencyMapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var models = currencyService
                .GetCurrencies()
                .Select(x => currencyMapper.ToModel(x))
                .ToList();

            return Ok(models);
        }

    }
}