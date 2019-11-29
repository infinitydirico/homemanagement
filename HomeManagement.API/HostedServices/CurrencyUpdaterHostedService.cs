using HomeManagement.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace HomeManagement.API.HostedServices
{
    public class CurrencyUpdaterHostedService : HostedService
    {
        public CurrencyUpdaterHostedService(ILogger<HostedService> logger,
            IServiceScopeFactory factory) 
            : base(logger, factory)
        {
        }

        public override int GetPeriodToRun() => 60 * 60 * 24;

        public override void Process()
        {
            var currencyService = GetService<ICurrencyService>();
            if (currencyService.IsUpToDate())
            {
                logger.LogInformation($"Currencies are already up to date.");
            }
            else
            {
                currencyService.UpdateCurrencies();
                logger.LogInformation($"Currencies were updated on: {DateTime.Now.ToString()}");
            }
        }
    }
}
