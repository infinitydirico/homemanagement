using System;

namespace HomeManagement.Domain
{
    public class Tax
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public DateTime DueDate { get; set; }

        public bool Paid { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}