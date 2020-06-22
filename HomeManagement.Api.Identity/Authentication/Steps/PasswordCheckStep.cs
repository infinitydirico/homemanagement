using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Authentication.Steps
{
    public class PasswordCheckStep : BaseStep
    {
        public PasswordCheckStep(AuthenticationStrategy authenticationStrategy) : base(authenticationStrategy)
        {
        }

        public override async Task<AuthenticationResult> Validate()
        {
            var password = authenticationStrategy.Cryptography.Decrypt(authenticationStrategy.UserModel.Password);

            if (!await authenticationStrategy.UserManager.CheckPasswordAsync(authenticationStrategy.IdentityUser, password))
            {
                await authenticationStrategy.UserManager.AccessFailedAsync(authenticationStrategy.IdentityUser);
                return AuthenticationResult.NotSucceeded("Invalid email or password.");
            }

            return AuthenticationResult.Succeeded();
        }
    }
}