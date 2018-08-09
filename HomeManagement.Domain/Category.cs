using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class Category
    {
        public Category()
        {
            UserCategories = new List<UserCategory>();
        }

        public Category(int id, string name, bool isActive, string icon, bool isDefault = false)
            :this()
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            Icon = icon;
            IsDefault = isDefault;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Icon { get; set; }

        public bool IsDefault { get; set; }

        public virtual List<UserCategory> UserCategories { get; set; }
    }
}