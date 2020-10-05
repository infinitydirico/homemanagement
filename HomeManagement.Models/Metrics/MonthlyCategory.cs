namespace HomeManagement.Models
{
    public class MonthlyCategory
    {
        public string Month { get; set; }

        public double Price { get; set; }

        public CategoryModel Category { get; set; }
    }
}
