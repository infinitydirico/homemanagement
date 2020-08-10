using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Authentication.Steps
{
    public class ExistsUserStep : BaseStep
    {
        public ExistsUserStep(AuthenticationStrategy authenticationStrategy) : base(authenticationStrategy)
        {
        }

        public override async Task<AuthenticationResult> Validate()
        {
            var user = await authenticationStrategy.UserManager.FindByEmailAsync(authenticationStrategy.UserModel.Email);

            if (user == null) return AuthenticationResult.NotSucceeded("Invalid email or password.");
            else
            {
                authenticationStrategy.IdentityUser = user;
                return AuthenticationResult.Succeeded();
            }
        }
    }
}