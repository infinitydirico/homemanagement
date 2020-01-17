using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeManagement.API.Storage.Services
{
    public class TemporaryFilesCleanerService : IHostedService, IDisposable
    {
        protected readonly ILogger<TemporaryFilesCleanerService> logger;
        protected Timer timer;

        public TemporaryFilesCleanerService(ILogger<TemporaryFilesCleanerService> logger)
        {
            this.logger = logger;
        }

        private void CleanFiles(object state)
        {
            logger.LogInformation($"{nameof(TemporaryFilesCleanerService)} Processing at: {DateTime.Now.ToString()}");

            var directory = $@"{Directory.GetCurrentDirectory()}{Core.Extensions.String.GetOsSlash()}temporary";

            if (!Directory.Exists(directory)) return;

            var files = Directory.EnumerateFiles(directory);

            var filesToDelete = files.Where(x => (DateTime.Now - File.GetLastAccessTime(x)).TotalMinutes > 30).ToList();

            logger.LogInformation($"Files to delete : {filesToDelete.Count}");

            foreach (var file in filesToDelete)
            {
                File.Delete(file);
                logger.LogInformation($"The file {file} has been deleted.");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(CleanFiles, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Stoping hosted service {nameof(TemporaryFilesCleanerService)}");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
