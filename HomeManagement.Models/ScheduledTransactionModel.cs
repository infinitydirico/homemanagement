using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class ScheduledTransactionModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public double Price { get; set; }

        [Required]
        public TransactionTypeModel TransactionType { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
