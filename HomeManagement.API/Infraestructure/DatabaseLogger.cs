using HomeManagement.API.Data.Entities;
using HomeManagement.API.Data.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace HomeManagement.API.Infraestructure
{
    public class DatabaseLogger : ILogger
    {
        private readonly IDataLogRepository dataLogRepository;

        public DatabaseLogger(IDataLogRepository dataLogRepository)
        {
            this.dataLogRepository = dataLogRepository;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (eventId.Id > 100) return;

            var message = formatter(state, exception);

            var logRecord = new DataLog
            {
                Level = logLevel,
                TimeStamp = DateTime.Now,
                Name = eventId.Name ?? string.Empty,
                Description = message,
            };

            dataLogRepository?.Add(logRecord);
        }
    }
}
