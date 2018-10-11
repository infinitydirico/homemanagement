using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class ReminderModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int DueDay { get; set; }

        public bool Active { get; set; }
    }
}
