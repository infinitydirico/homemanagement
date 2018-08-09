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

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
