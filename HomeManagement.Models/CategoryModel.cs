namespace HomeManagement.Models
{
    public class CategoryModel
    {
        public CategoryModel() { }

        public CategoryModel(int id, string name, bool isActive, string icon, bool isDefault = false)
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
    }
}
