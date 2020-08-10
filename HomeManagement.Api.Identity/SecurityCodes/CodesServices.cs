using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Api.Identity.SecurityCodes
{
    public class CodesServices : ICodesServices
    {
        protected List<UserCode> userCodes = new List<UserCode>();
        private static Random random = new Random();

        public UserCode GetUserCode(string email) => userCodes.FirstOrDefault(x => x.Email.Equals(email));

        public void LoadUsers(UserManager<IdentityUser> userManager)
        {
            var users = userManager
                .Users
                .Where(x => x.TwoFactorEnabled)
                .ToList();

            var diffUsers = (from u in users
                             where !(from uc in userCodes
                                     select uc.Email).Contains(u.Email)
                             select u).ToList();

            userCodes.AddRange(diffUsers.Select(x => new UserCode(random) { Email = x.Email }));
        }        
    }
}