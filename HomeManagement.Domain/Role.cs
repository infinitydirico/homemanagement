using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<UserInRole> UsersInRoles { get; set; }
    }
}