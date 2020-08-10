using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Authentication.Steps
{
    public abstract class BaseStep
    {
        protected readonly AuthenticationStrategy authenticationStrategy;

        public BaseStep(AuthenticationStrategy authenticationStrategy)
        {
            this.authenticationStrategy = authenticationStrategy;
        }

        public abstract Task<AuthenticationResult> Validate();
    }
}
