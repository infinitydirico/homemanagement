using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class TokenPasswordModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
