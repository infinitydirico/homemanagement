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
            nameof(IsDefault)
        };

        public Category()
        {
            UserCategories = new List<UserCategory>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Icon { get; set; }

        public bool IsDefault { get; set; }

        public virtual List<UserCategory> UserCategories { get; set; }

        public IList<string> GetProperties() => exportableHeaders;
    }
}