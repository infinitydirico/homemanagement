using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Authentication.Steps
{
    public class TwoFactorStep : BaseStep
    {
        public TwoFactorStep(AuthenticationStrategy authenticationStrategy) : base(authenticationStrategy)
        {
        }

        public override async Task<AuthenticationResult> Validate()
        {
            var twoFactorEnabled = await authenticationStrategy.UserManager.GetTwoFactorEnabledAsync(authenticationStrategy.IdentityUser);

            if (twoFactorEnabled && !authenticationStrategy.MobileAppToken.Equals(authenticationStrategy.MobileHeader))
            {
                var userCode = authenticationStrategy.CodesServices.GetUserCode(authenticationStrategy.UserModel.Email);

                if (userCode.Code.Equals(default)) return AuthenticationResult.NotSucceeded("Code not found.");

                if (userCode.Code != authenticationStrategy.UserModel.SecurityCode) return AuthenticationResult.NotSucceeded("Invalid code.");
            }

            return AuthenticationResult.Succeeded();
        }
    }
}