using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
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
        private readonly IUserRepository userRepository;
        private readonly IPreferencesRepository preferencesRepository;

        public PreferencesController(IUserRepository userRepository, IPreferencesRepository preferencesRepository)
        {
            this.userRepository = userRepository;
            this.preferencesRepository = preferencesRepository;
        }

        [HttpPost("changelanguage")]
        public IActionResult ChangeLanguage([FromBody]string language)
        {
            if (!LocalizationLanguage.IsValid(language)) return BadRequest();

            var emailClaim = HttpContext.GetEmailClaim();

            var userPreference = (from user in userRepository.All
                                  join preference in preferencesRepository.All
                                  on user.Id equals preference.UserId
                                  where user.Email.Equals(emailClaim.Value)
                                  select preference).FirstOrDefault();

            userPreference.Language = language;

            preferencesRepository.Update(userPreference);

            return Ok();
        }
    }
}
