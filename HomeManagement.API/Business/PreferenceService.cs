using HomeManagement.API.Data;
using HomeManagement.API.Services;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
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
        private readonly ICurrencyMapper currencyMapper;
        private readonly ICurrencyService currencyService;
        private readonly IUserSessionService userService;

        private const string LanguageKey = "Language";
        private const string PreferredCurrency = "PreferredCurrency";
        private const string UserCountry = "UserCountry";

        public PreferenceService(IUserRepository userRepository,
            IPreferencesRepository preferencesRepository,
            ICategoryRepository categoryRepository,
            IUserCategoryRepository userCategoryRepository,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICurrencyMapper currencyMapper,
            ICurrencyService currencyService,
            IUserSessionService userService)
        {
            this.userRepository = userRepository;
            this.preferencesRepository = preferencesRepository;
            this.categoryRepository = categoryRepository;
            this.userCategoryRepository = userCategoryRepository;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.currencyMapper = currencyMapper;
            this.currencyService = currencyService;
            this.userService = userService;
        }

        public void ChangeLanguage(string language)
        {
            var user = userService.GetAuthenticatedUser();

            var userPreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id)) ?? new Preferences();

            userPreference.Key = LanguageKey;
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

            preferencesRepository.Commit();

            UpdateUserCategories(user, language);

            UpdateTransactions(user, language);
        }

        public string GetUserLanguage(int userId)
        {
            var languagePreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(LanguageKey));

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
            categoryRepository.Commit();

            var defaultCategories = CategoryInitializer.GetDefaultCategories(new System.Globalization.CultureInfo(language));

            foreach (var category in defaultCategories)
            {
                categoryRepository.Add(category, user);
            }
            categoryRepository.Commit();
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

            transactionRepository.Commit();
        }

        public void ChangeCurrency(CurrencyModel currency)
        {
            var user = userService.GetAuthenticatedUser();
            var currencyPreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals(PreferredCurrency));

            if (currencyPreference == null)
            {
                currencyPreference = new Preferences
                {
                    UserId = user.Id,
                    Key = PreferredCurrency,
                    Value = currency.Name,
                };

                preferencesRepository.Add(currencyPreference);
            }
            else
            {
                currencyPreference.Value = currency.Name;
                preferencesRepository.Update(currencyPreference);
            }

            preferencesRepository.Commit();
        }

        public IEnumerable<CurrencyModel> GetCurrencies() => currencyService.GetCurrencies().Select(x => currencyMapper.ToModel(x));

        public CurrencyModel GetPreferredCurrency()
        {
            var user = userService.GetAuthenticatedUser();
            var preferredCurrency = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals(PreferredCurrency));
            var currency = GetCurrencies().FirstOrDefault(x => x.Name.Equals(preferredCurrency.Value));
            return currency;
        }

        public void SaveCountry(string country)
        {
            var user = userService.GetAuthenticatedUser();
            var countryPreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals(UserCountry));

            if (countryPreference == null)
            {
                countryPreference = new Preferences
                {
                    UserId = user.Id,
                    Key = UserCountry,
                    Value = country,
                };

                preferencesRepository.Add(countryPreference);
            }
            else
            {
                countryPreference.Value = country;
                preferencesRepository.Update(countryPreference);
            }

            preferencesRepository.Commit();
        }

        public string GetUserCountry()
        {
            var user = userService.GetAuthenticatedUser();
            var countryPreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals(UserCountry));
            return countryPreference?.Value;
        }

        public string GetUserCountryCode()
        {
            var country = GetUserCountry();
            var parts = country.Split(" ");
            if (parts.Count() > 1)
            {
                return parts.First().Substring(0, 1) + parts.Last().Substring(0, 1);
            }
            else
            {
                return country.ToUpper().Substring(0, 2);
            }
        }
    }

    public interface IPreferenceService
    {
        void ChangeLanguage(string language);

        string GetUserLanguage(int userId);

        void ChangeCurrency(CurrencyModel currency);

        CurrencyModel GetPreferredCurrency();

        IEnumerable<CurrencyModel> GetCurrencies();

        void SaveCountry(string country);

        string GetUserCountry();

        string GetUserCountryCode();
    }
}
