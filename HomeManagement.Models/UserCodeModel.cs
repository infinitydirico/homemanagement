using System;
using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class UserCodeModel
    {
        [Required]
        public string Email { get; set; }

        public int Code { get; set; }

        public DateTime Expiration { get; set; }
    }
}
