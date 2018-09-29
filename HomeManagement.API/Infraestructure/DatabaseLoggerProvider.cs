using HomeManagement.API.Data;
using HomeManagement.API.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace HomeManagement.API.Infraestructure
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers = new ConcurrentDictionary<string, DatabaseLogger>();
        private readonly IServiceProvider serviceProvider;

        public DatabaseLoggerProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dataLogRepository = scope.ServiceProvider.GetRequiredService<IDataLogRepository>();
                return _loggers.GetOrAdd(categoryName, name => new DatabaseLogger(dataLogRepository));
            }
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
