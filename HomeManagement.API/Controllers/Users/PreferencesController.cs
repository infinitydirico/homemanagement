using HomeManagement.API.Data;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Localization;
using HomeManagement.Models;
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
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserCategoryRepository userCategoryRepository;

        public PreferencesController(IUserRepository userRepository,
            IPreferencesRepository preferencesRepository,
            ICategoryRepository categoryRepository,
            IUserCategoryRepository userCategoryRepository)
        {
            this.userRepository = userRepository;
            this.preferencesRepository = preferencesRepository;
            this.categoryRepository = categoryRepository;
            this.userCategoryRepository = userCategoryRepository;
        }

        [HttpPost("changelanguage/{language}")]
        public IActionResult ChangeLanguage(string language)
        {
            if (!LocalizationLanguage.IsValid(language)) return BadRequest();

            var emailClaim = HttpContext.GetEmailClaim();

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(emailClaim.Value));

            var userPreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id));

            userPreference.Language = language;

            preferencesRepository.Update(userPreference);

            UpdateUserCategories(user, language);

            return Ok();
        }

        private void UpdateUserCategories(User user, string language)
        {
            var userCategories = (from category in categoryRepository.All
                                  join userCategory in userCategoryRepository.All
                                  on category.Id equals userCategory.CategoryId
                                  where userCategory.UserId.Equals(user.Id)
                                  select category).ToList();

            foreach (var category in userCategories)
            {
                categoryRepository.Remove(category.Id, user);
            }

            var defaultCategories = CategoryInitializer.GetDefaultCategories(new System.Globalization.CultureInfo(language));

            foreach (var category in defaultCategories)
            {
                categoryRepository.Add(category, user);
            }
        }
    }
}
