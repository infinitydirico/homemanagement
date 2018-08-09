using HomeManagement.Domain;

namespace HomeManagement.Models
{
    public class UserInRoleModel
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }

    public class EnRollUserModel
    {
        public string Email { get; set; }

        public string RoleName { get; set; }
    }
}
