using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeManagement.API.HostedServices
{
    public abstract class HostedService : IHostedService, IDisposable
    {
        protected readonly IServiceScopeFactory factory;
        protected readonly ILogger<HostedService> logger;
        protected Timer timer;

        public HostedService(ILogger<HostedService> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }

        public abstract void Process();

        public int GetPeriodToRun()
        {
            var configuration = GetService<IConfiguration>();
            var value = configuration.GetValue<int>($"HostedServices:{GetType().Name}");
            return value;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Starting hosted service {GetType().Name}");

            var period = GetPeriodToRun();
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(period));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Stoping hosted service {GetType().Name}");

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

        protected virtual void DoWork(object state)
        {
            try
            {
                logger.LogInformation($"{GetType().Name} Processing at: {DateTime.Now.ToString()}");
                Process();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name} Failed at: {DateTime.Now.ToString()}");
            }
        }
    }
}
