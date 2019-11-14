using HomeManagement.API.Data;
using HomeManagement.API.Services;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.API.Business
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly ICurrencyMapper currencyMapper;
        private readonly ICurrencyService currencyService;
        private readonly IUserSessionService userService;

        private const string LanguageKey = "Language";
        private const string PreferredCurrency = "PreferredCurrency";
        private const string UserCountry = "UserCountry";

        public PreferenceService(IRepositoryFactory repositoryFactory,
            ICurrencyMapper currencyMapper,
            ICurrencyService currencyService,
            IUserSessionService userService)
        {
            this.repositoryFactory = repositoryFactory;
            this.currencyMapper = currencyMapper;
            this.currencyService = currencyService;
            this.userService = userService;
        }

        public void ChangeLanguage(string language)
        {
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
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

                Task.Run(() =>
                {
                    UpdateUserCategories(user, language);

                    UpdateTransactions(user);
                });
            }
        }

        public string GetUserLanguage(int userId)
        {
            var preferencesRepository = repositoryFactory.CreatePreferencesRepository();

            var languagePreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(LanguageKey));

            return languagePreference?.Value ?? "en";
        }

        private void UpdateUserCategories(User user, string language)
        {
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            {
                var userCategories = categoryRepository.GetUserCategories(user.Email);

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
        }

        private void UpdateTransactions(User user)
        {
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            {
                var transactionsWithOldCategories = transactionRepository.GetByUser(user.Email);

                var userCategories = categoryRepository.GetActiveUserCategories(user.Email);

                foreach (var transaction in transactionsWithOldCategories)
                {
                    var oldCategory = categoryRepository.FirstOrDefault(x => x.Id.Equals(transaction.CategoryId));

                    var newCategory = userCategories.First(x => x.Icon.Equals(oldCategory.Icon));

                    transaction.CategoryId = newCategory.Id;

                    transactionRepository.Update(transaction);
                }

                transactionRepository.Commit();
            }
        }

        public void ChangeCurrency(CurrencyModel currency)
        {
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
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
        }

        public IEnumerable<CurrencyModel> GetCurrencies() => currencyService.GetCurrencies().Select(x => currencyMapper.ToModel(x));

        public CurrencyModel GetPreferredCurrency()
        {
            var preferencesRepository = repositoryFactory.CreatePreferencesRepository();
            var user = userService.GetAuthenticatedUser();
            var preferredCurrency = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals(PreferredCurrency));
            var currencyRepository = repositoryFactory.CreateCurrencyRepository();
            var currency = currencyRepository.FirstOrDefault(x => x.Name.Equals(preferredCurrency.Value));
            return currencyMapper.ToModel(currency);
        }

        public void SaveCountry(string country)
        {
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
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
        }

        public string GetUserCountry()
        {
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
            {
                var user = userService.GetAuthenticatedUser();
                var countryPreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals(UserCountry));
                return countryPreference?.Value;
            }
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
