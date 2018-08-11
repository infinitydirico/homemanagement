using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class UserModel
    {
        public UserModel() { }

        public UserModel(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
