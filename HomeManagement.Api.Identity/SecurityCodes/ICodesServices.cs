using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Api.Identity.SecurityCodes
{
    public interface ICodesServices
    {
        UserCode GetUserCode(string email);

        void LoadUsers(UserManager<IdentityUser> userManager);
    }
}
