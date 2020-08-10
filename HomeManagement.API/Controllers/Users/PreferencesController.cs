using HomeManagement.API.Business;
using HomeManagement.Api.Core.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Domain;
using HomeManagement.Localization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HomeManagement.API.Controllers.Users
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Preferences")]
    public class PreferencesController : Controller
    {
        private readonly IPreferenceService preferenceService;

        public PreferencesController(IPreferenceService preferenceService)
        {
            this.preferenceService = preferenceService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var preferredCurrency = preferenceService.GetPreferredCurrency();
            var language = preferenceService.GetUserLanguage();
            var enableDailyBackups = preferenceService.GetEnableDailyBackups();

            return Ok(new
            {
                Currency = preferredCurrency.Name,
                Language = language,
                EnableDailyBackups = enableDailyBackups
            });
        }

        [HttpGet("{preference}")]
        public IActionResult Get(string preference)
        {
            var preferences = preferenceService.GetAllUserPreferences();

            if (!preferences.Any(p => p.Key.Equals(preference))) return NotFound();

            var value = preferences.FirstOrDefault(x => x.Key.Equals(preference));

            return Ok(value);
        }

        [HttpPost("changelanguage/{language}")]
        public IActionResult ChangeLanguage(string language)
        {
            if (!LocalizationLanguage.IsValid(language)) return BadRequest();

            var emailClaim = HttpContext.GetEmailClaim();

            preferenceService.ChangeLanguage(language);

            return Ok();
        }

        [HttpPost("changepreferredcurrency/{currency}")]
        public IActionResult ChangePreferredCurrency(string currency)
        {
            var emailClaim = HttpContext.GetEmailClaim();

            var currencyModel = preferenceService.GetCurrencies().First(x => x.Name.Equals(currency));

            preferenceService.ChangeCurrency(currencyModel);

            return Ok();
        }

        [HttpPost("savecountry/{country}")]
        public IActionResult SaveCountry(string country)
        {
            preferenceService.SaveCountry(country);
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Preferences preference)
        {
            preferenceService.Save(preference);

            return Ok();
        }
    }
}
