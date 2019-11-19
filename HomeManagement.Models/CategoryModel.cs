namespace HomeManagement.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Icon { get; set; }

        public bool Measurable { get; set; }

        public int UserId { get; set; }
    }
}
