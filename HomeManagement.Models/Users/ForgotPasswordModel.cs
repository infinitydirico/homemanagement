using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        public string Email { get; set; }
    }
}
