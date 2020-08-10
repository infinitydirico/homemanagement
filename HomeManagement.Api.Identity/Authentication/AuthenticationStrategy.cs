using HomeManagement.Api.Identity.Authentication.Steps;
using HomeManagement.Api.Identity.Controllers;
using HomeManagement.Api.Identity.SecurityCodes;
using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Authentication
{
    public class AuthenticationStrategy
    {
        public AuthenticationStrategy(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICryptography cryptography,
            IConfiguration configuration,
            ILogger<AuthenticationController> logger,
            ICodesServices codesServices)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Cryptography = cryptography;
            Configuration = configuration;
            Logger = logger;
            CodesServices = codesServices;
            MobileAppToken = configuration["MobileApp:Token"];
        }

        protected internal UserManager<IdentityUser> UserManager { get; private set; }
        protected internal SignInManager<IdentityUser> SignInManager { get; private set; }
        protected internal ICryptography Cryptography { get; private set; }
        protected internal IConfiguration Configuration { get; private set; }
        protected internal ILogger<AuthenticationController> Logger { get; private set; }
        protected internal ICodesServices CodesServices { get; private set; }
        protected internal string MobileAppToken { get; private set; }
        protected internal UserModel UserModel { get; private set; }
        protected internal IdentityUser IdentityUser { get; set; }
        protected internal string MobileHeader { get; set; }

        public async Task<AuthenticationResult> Authenticate(UserModel userModel)
        {
            var steps = new List<BaseStep>
            {
                new ExistsUserStep(this),
                new IsLockedStep(this),
                new PasswordCheckStep(this),
                new SignInStep(this),
                new CreateTokenStep(this)
            };

            return await InternalAuthenticate(userModel, steps);
        }

        public async Task<AuthenticationResult> Authenticate(UserModel userModel, string mobileHeader)
        {
            MobileHeader = mobileHeader;

            var steps = new List<BaseStep>
            {
                new ExistsUserStep(this),
                new IsLockedStep(this),
                new PasswordCheckStep(this),
                new TwoFactorStep(this),
                new SignInStep(this),
                new CreateTokenStep(this)
            };

            return await InternalAuthenticate(userModel, steps);
        }

        public async Task<UserModel> GetAuthenticatedUser() => await Task.FromResult(UserModel);

        private async Task<AuthenticationResult> InternalAuthenticate(UserModel userModel, List<BaseStep> steps)
        {
            UserModel = userModel;

            foreach (var step in steps)
            {
                var result = await step.Validate();
                if (!result.Succeed) return result;
            }

            return AuthenticationResult.Succeeded();
        }
    }
}