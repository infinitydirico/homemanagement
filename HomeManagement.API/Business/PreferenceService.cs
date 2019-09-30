using HomeManagement.API.Data;
using HomeManagement.Data;
using HomeManagement.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IUserRepository userRepository;
        private readonly IPreferencesRepository preferencesRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserCategoryRepository userCategoryRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountRepository accountRepository;

        public PreferenceService(IUserRepository userRepository,
            IPreferencesRepository preferencesRepository,
            ICategoryRepository categoryRepository,
            IUserCategoryRepository userCategoryRepository,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            this.userRepository = userRepository;
            this.preferencesRepository = preferencesRepository;
            this.categoryRepository = categoryRepository;
            this.userCategoryRepository = userCategoryRepository;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
        }

        public void ChangeLanguage(string userEmail, string language)
        {
            var user = userRepository.FirstOrDefault(x => x.Email.Equals(userEmail));

            var userPreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id)) ?? new Preferences();

            userPreference.Key = "Language";
            userPreference.Value = language;
            userPreference.UserId = user.Id;

            if (userPreference.Id > 0)
            {
                preferencesRepository.Update(userPreference);
            }
            else
            {
                preferencesRepository.Add(userPreference);
            }

            UpdateUserCategories(user, language);

            UpdateTransactions(user, language);
        }

        public string GetUserLanguage(int userId)
        {
            var languagePreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals("Language"));

            return languagePreference?.Value ?? "en";
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

        private void UpdateTransactions(User user, string language)
        {
            var transactionsWithOldCategories = (from transaction in transactionRepository.All
                                                 join account in accountRepository.All
                                                 on transaction.AccountId equals account.Id
                                                 where account.UserId.Equals(user.Id) &&
                                                         userCategoryRepository.All.Any(x => x.CategoryId != transaction.CategoryId)
                                                 select transaction);

            foreach (var transaction in transactionsWithOldCategories)
            {
                var oldCategory = categoryRepository.FirstOrDefault(x => x.Id.Equals(transaction.CategoryId));

                var newCategory = (from userCategory in userCategoryRepository.All
                                   join category in categoryRepository.All
                                   on userCategory.CategoryId equals category.Id
                                   where userCategory.UserId.Equals(user.Id) &&
                                            category.Icon.Equals(oldCategory.Icon)
                                   select category).FirstOrDefault();

                transaction.CategoryId = newCategory.Id;

                transactionRepository.Update(transaction);
            }
        }
    }

    public interface IPreferenceService
    {
        void ChangeLanguage(string userEmail, string language);

        string GetUserLanguage(int userId);
    }
}
