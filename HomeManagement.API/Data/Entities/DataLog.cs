using Microsoft.Extensions.Logging;
using System;

namespace HomeManagement.API.Data.Entities
{
    public class DataLog
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public LogLevel Level { get; set; }

        public DateTime TimeStamp { get; set; }
    }    
}
