using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Authentication.Steps
{
    public class SignInStep : BaseStep
    {
        public SignInStep(AuthenticationStrategy authenticationStrategy) : base(authenticationStrategy)
        {
        }

        public override async Task<AuthenticationResult> Validate()
        {
            var password = authenticationStrategy.Cryptography.Decrypt(authenticationStrategy.UserModel.Password);

            var result = await authenticationStrategy.SignInManager.PasswordSignInAsync(authenticationStrategy.UserModel.Email, password, true, false);

            if (!result.Succeeded) return AuthenticationResult.NotSucceeded("Invalid email or password.");

            return AuthenticationResult.Succeeded();
        }
    }
}