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
        private readonly ICurrencyRepository currencyRepository;
        private readonly IConfiguration configuration;
    
        public CurrencyService(ICurrencyRepository currencyRepository, IConfiguration configuration)
        {
            this.currencyRepository = currencyRepository;
            this.configuration = configuration;
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

            return currencyRepository.All.ToList();
        }

        private bool IsUpToDate()
        {
            var currencies = currencyRepository.All.ToList();

            return currencies.All(x => (DateTime.Now - x.ChangeStamp).TotalDays > 1.0);
        }

        private void UpdateCurrencies()
        {
            var apiCurrencies = GetApiCurrencies()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            var currencies = (from dbCurrency in currencyRepository.All
                              join c in apiCurrencies
                              on dbCurrency.Name equals c.Name
                              select new Currency
                              {
                                  Id = dbCurrency.Id,
                                  Name = dbCurrency.Name,
                                  Value = c.Value,
                                  ChangeStamp = c.ChangeStamp
                              }).ToList();

            foreach (var currency in currencies)
            {
                currencyRepository.Update(currency);
            }
        }

        private async Task<List<Currency>> GetApiCurrencies()
        {
            using(var httpClient = new HttpClient())
            {
                var baseUrl = configuration
                    .GetSection("CurrenciesSettings")
                    .GetValue<string>("API");

                var apiKey = configuration
                    .GetSection("CurrenciesSettings")
                    .GetValue<string>("AppId");

                httpClient.BaseAddress = new Uri(baseUrl);

                var response = await httpClient.GetAsync($"latest.json?app_id={apiKey}");

                response.EnsureSuccessStatusCode();

                var values = JsonConvert.DeserializeObject<Latest>(await response.Content.ReadAsStringAsync());
                return values
                    .Rates
                    .Where(x => x.Name.Equals("USD") || x.Name.Equals("ARS") || x.Name.Equals("EUR"))
                    .Select(x => new Currency
                    {
                        Name = x.Name,
                        Value = x.Value,
                        ChangeStamp = DateTime.Now
                    })
                    .ToList();
            }
        }
    }

    public class Latest
    {
        public List<Rate> Rates { get; set; }
    }

    public class Rate
    {
        public string Name { get; set; }

        public double Value { get; set; }
    }
}
