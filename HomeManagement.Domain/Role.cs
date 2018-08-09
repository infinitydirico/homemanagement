using System.Collections.Generic;

namespace HomeManagement.Domain
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public virtual List<UserInRole> UsersInRoles { get; set; }
    }
}