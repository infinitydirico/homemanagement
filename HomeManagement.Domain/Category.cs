using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Icon { get; set; }

        public bool IsDefault { get; set; }

        public virtual List<UserCategory> UserCategories { get; set; }
    }
}