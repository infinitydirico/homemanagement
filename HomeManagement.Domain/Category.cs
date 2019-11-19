using HomeManagement.Contracts;
using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class Category : IExportable
    {
        IList<string> exportableHeaders = new List<string>
        {
            nameof(Name),
            nameof(IsActive),
            nameof(Icon),
            nameof(Measurable)
        };

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Measurable { get; set; }

        public bool IsActive { get; set; }

        public string Icon { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public IList<string> GetProperties() => exportableHeaders;
    }
}