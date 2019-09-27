using HomeManagement.API.Data;
using HomeManagement.API.Extensions;
using HomeManagement.API.Filters;
using HomeManagement.Data;
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
    [Persistable]
    public class PreferencesController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IPreferencesRepository preferencesRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserCategoryRepository userCategoryRepository;
        private readonly Data.Repositories.ITransactionRepository transactionRepository;
        private readonly IAccountRepository accountRepository;

        public PreferencesController(IUserRepository userRepository,
            IPreferencesRepository preferencesRepository,
            ICategoryRepository categoryRepository,
            IUserCategoryRepository userCategoryRepository,
            Data.Repositories.ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            this.userRepository = userRepository;
            this.preferencesRepository = preferencesRepository;
            this.categoryRepository = categoryRepository;
            this.userCategoryRepository = userCategoryRepository;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
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

            UpdateCharges(user, language);

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

        private void UpdateCharges(User user, string language)
        {
            var chargesWithOldCategories = (from charge in transactionRepository.All
                                            join account in accountRepository.All
                                            on charge.AccountId equals account.Id
                                            where account.UserId.Equals(user.Id) &&
                                                    userCategoryRepository.All.Any(x => x.CategoryId != charge.CategoryId)
                                            select charge);

            foreach (var charge in chargesWithOldCategories)
            {
                var oldCategory = categoryRepository.FirstOrDefault(x => x.Id.Equals(charge.CategoryId));

                var newCategory = (from userCategory in userCategoryRepository.All
                                   join category in categoryRepository.All
                                   on userCategory.CategoryId equals category.Id
                                   where userCategory.UserId.Equals(user.Id) &&
                                            category.Icon.Equals(oldCategory.Icon)
                                   select category).FirstOrDefault();

                charge.CategoryId = newCategory.Id;

                transactionRepository.Update(charge);
            }
        }
    }
}
