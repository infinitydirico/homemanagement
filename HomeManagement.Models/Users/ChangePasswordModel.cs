using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
