using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class UserIdentityModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
