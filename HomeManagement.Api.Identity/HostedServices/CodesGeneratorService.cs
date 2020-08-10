using HomeManagement.Api.Identity.SecurityCodes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.HostedServices
{
    public class CodesGeneratorService : IHostedService, IDisposable
    {
        protected readonly IServiceScopeFactory factory;
        protected readonly ILogger<CodesGeneratorService> logger;
        protected readonly ICodesServices codesServices;
        protected Timer timer;

        public CodesGeneratorService(ILogger<CodesGeneratorService> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
            codesServices = GetService<ICodesServices>();
        }

        private void GenerateCodes(object state)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                codesServices.LoadUsers(GetService<UserManager<IdentityUser>>());

                watch.Stop();
                logger.LogInformation($"Time passed: {watch.Elapsed}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(CodesGeneratorService)} Failed at: {DateTime.Now}");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Starting service {nameof(CodesGeneratorService)}");

            var period = 120;
            timer = new Timer(GenerateCodes, null, TimeSpan.Zero, TimeSpan.FromSeconds(period));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Stoping service {nameof(CodesGeneratorService)}");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public TService GetService<TService>()
        {
            return factory.CreateScope().ServiceProvider.GetRequiredService<TService>();
        }
    }
}
