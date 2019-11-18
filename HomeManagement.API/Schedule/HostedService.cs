using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeManagement.API.Schedule
{
    public abstract class HostedService : IHostedService, IDisposable
    {
        protected readonly IServiceScopeFactory factory;
        protected readonly ILogger<HostedService> logger;
        private Timer timer;

        public HostedService(ILogger<HostedService> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }

        public abstract void Process();

        public abstract int GetPeriodToRun();

        public Task StartAsync(CancellationToken cancellationToken)
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

        private void DoWork(object state)
        {
            logger.LogInformation($"{GetType().Name} Processing at: {DateTime.Now.ToString()}");
            Process();
        }
    }
}
