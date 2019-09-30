using HomeManagement.API.Business;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Localization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers.Users
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Preferences")]
    [Persistable]
    public class PreferencesController : Controller
    {
        private readonly IPreferenceService preferenceService;

        public PreferencesController(IPreferenceService preferenceService)
        {
            this.preferenceService = preferenceService;
        }

        [HttpPost("changelanguage/{language}")]
        public IActionResult ChangeLanguage(string language)
        {
            if (!LocalizationLanguage.IsValid(language)) return BadRequest();

            var emailClaim = HttpContext.GetEmailClaim();

            preferenceService.ChangeLanguage(emailClaim.Value, language);

            return Ok();
        }
    }
}
