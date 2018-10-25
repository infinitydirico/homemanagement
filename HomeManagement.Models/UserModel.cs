﻿using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Token { get; set; }

        public string Language { get; set; }
    }
}
