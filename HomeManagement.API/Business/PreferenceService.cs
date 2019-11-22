using HomeManagement.API.Services;
using HomeManagement.Business.Contracts;
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
            }
        }

        public string GetUserLanguage(int userId)
        {
            var preferencesRepository = repositoryFactory.CreatePreferencesRepository();

            var languagePreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(LanguageKey));

            return languagePreference?.Value ?? "en";
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

        public IEnumerable<CurrencyModel> GetCurrencies() => currencyService.GetCurrencies().Select(x => currencyMapper.ToModel(x)).ToList();

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
