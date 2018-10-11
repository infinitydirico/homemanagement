using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class AccountModel
    {
        public AccountModel() { }

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public double Balance { get; set; }

        public bool ExcludeFromStatistics { get; set; }

        public bool IsCash { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
