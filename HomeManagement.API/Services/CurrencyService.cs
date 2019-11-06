using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeManagement.API.Services
{
    public interface ICurrencyService
    {
        Currency GetCurrency(string name);

        List<Currency> GetCurrencies();
    }

    public class CurrencyService : ICurrencyService
    {
        private const string ApiUrlKey = "CurrencyService_API";
        private const string AppIdKey = "CurrencyService_AppId";
        private readonly IConfigurationSettingsRepository configurationSettingsRepository;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IConfiguration configuration;
        private readonly List<string> supportedCurrencies = new List<string> { "USD", "EUR", "ARS" };
        private readonly IPlatformContext platformContext;

        public CurrencyService(ICurrencyRepository currencyRepository,
            IConfigurationSettingsRepository configurationSettingsRepository,
            IPlatformContext platformContext)
        {
            this.currencyRepository = currencyRepository;
            this.configurationSettingsRepository = configurationSettingsRepository;
            this.platformContext = platformContext;
        }

        public Currency GetCurrency(string name)
        {
            if (!IsUpToDate())
            {
                UpdateCurrencies();
            }

            return currencyRepository.FirstOrDefault(x => x.Name.Equals(name));
        }

        public List<Currency> GetCurrencies()
        {
            if (!IsUpToDate())
            {
                UpdateCurrencies();
            }

            return currencyRepository.GetAll().ToList();
        }

        private bool IsUpToDate()
        {
            var currencies = currencyRepository.GetAll().ToList();

            return currencies.All(x => (DateTime.Now - x.ChangeStamp).TotalDays < 1.0);
        }

        private void UpdateCurrencies()
        {
            var apiCurrencies = GetApiCurrencies()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            var currencies = currencyRepository.GetAll().ToList();

            foreach (var currency in currencies)
            {
                var currenctApiValue = apiCurrencies.FirstOrDefault(x => x.Name.Equals(currency.Name));
                currency.ChangeStamp = DateTime.Now;
                currency.Value = currenctApiValue.Value;

                currencyRepository.Update(currency);
            }

            platformContext.Commit();
        }

        private async Task<List<Currency>> GetApiCurrencies()
        {
            if (!IsConfigued()) return CreateDefault().ToList();

            using (var httpClient = new HttpClient())
            {
                var baseUrl = configurationSettingsRepository.GetValue(ApiUrlKey);
                var apiKey = configurationSettingsRepository.GetValue(AppIdKey);

                httpClient.BaseAddress = new Uri(baseUrl);

                var response = await httpClient.GetAsync($"latest.json?app_id={apiKey}");

                response.EnsureSuccessStatusCode();

                var content = (await response.Content.ReadAsStringAsync()).Replace(@"\n", "");

                var values = JsonConvert.DeserializeObject<Latest>(content);

                return values
                    .Rates
                    .Where(x => supportedCurrencies.Any(y => y.Equals(x.Key)))
                    .Select(x => new Currency
                    {
                        Name = x.Key,
                        Value = x.Value,
                        ChangeStamp = DateTime.Now
                    })
                    .ToList();
            }
        }

        private bool IsConfigued()
        {
            var exists = configurationSettingsRepository.Exists(ApiUrlKey) && configurationSettingsRepository.Exists(AppIdKey);

            if (exists)
            {
                var apiValue = configurationSettingsRepository.GetValue(ApiUrlKey);
                var appIdValue = configurationSettingsRepository.GetValue(AppIdKey);

                return !string.IsNullOrEmpty(apiValue) && !string.IsNullOrEmpty(appIdValue);
            }
            return false;
        }

        private IEnumerable<Currency> CreateDefault()
        {
            return supportedCurrencies.Select(x => new Currency
            {
                Name = x,
                ChangeStamp = DateTime.Now,
                Value = 0
            });
        }
    }

    public class Latest
    {
        [JsonProperty("rates")]
        public Dictionary<string, double> Rates { get; set; }
    }

    public class Rate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name")]
        public double Value { get; set; }
    }
}
