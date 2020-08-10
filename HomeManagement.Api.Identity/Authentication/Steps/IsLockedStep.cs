using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Authentication.Steps
{
    public class IsLockedStep : BaseStep
    {
        public IsLockedStep(AuthenticationStrategy authenticationStrategy) : base(authenticationStrategy)
        {
        }

        public override async Task<AuthenticationResult> Validate()
        {
            if (await authenticationStrategy.UserManager.IsLockedOutAsync(authenticationStrategy.IdentityUser))
            {
                return AuthenticationResult.NotSucceeded($"The user {authenticationStrategy.IdentityUser.Email} has been locked out.");
            }
            return AuthenticationResult.Succeeded();
        }
    }
}