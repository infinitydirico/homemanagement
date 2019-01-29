using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using HomeManagement.API.Data;
using HomeManagement.API.Data.Entities;
using HomeManagement.API.Extensions;
using HomeManagement.Contracts;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeManagement.API.Controllers.Users
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Register")]
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly JwtSecurityTokenHandler jwtSecurityToken = new JwtSecurityTokenHandler();
        private readonly IAccountRepository accountRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IPreferencesRepository preferencesRepository;
        private readonly ICryptography cryptography;

        public RegisterController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IAccountRepository accountRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IServiceScopeFactory serviceScopeFactory,
            IPreferencesRepository preferencesRepository,
            ICryptography cryptography)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.accountRepository = accountRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
            this.serviceScopeFactory = serviceScopeFactory;
            this.preferencesRepository = preferencesRepository;
            this.cryptography = cryptography;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel user)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await userManager.CreateAsync(new ApplicationUser
            {
                Email = user.Email,
                UserName = user.Email
            }, cryptography.Decrypt(user.Password));

            if (result.Succeeded)
            {
                var applicationUser = await userManager.FindByEmailAsync(user.Email);

#pragma warning disable 4014

                var userEntity = new User
                {
                    Email = applicationUser.Email,
                };

                await userRepository.AddAsync(userEntity);

                var cultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();

                accountRepository.Add(new Account
                {
                    UserId = userEntity.Id,
                    Name = cultureFeature?.RequestCulture?.Culture != null ? cultureFeature.RequestCulture.Culture.IsEnglish() ? "Cash" : "Efectivo" : "Cash",
                    AccountType = Domain.AccountType.Cash,
                    CurrencyId = 1
                });

                var defaultCategories = CategoryInitializer.GetDefaultCategories(cultureFeature?.RequestCulture?.Culture);

                foreach (var category in defaultCategories)
                {
                    categoryRepository.Add(category, userEntity);
                }

                preferencesRepository.Add(new Preferences
                {
                    UserId = userEntity.Id,
                    Language = user.Language
                });
#pragma warning restore 4014

                return Ok();
            }

            else return BadRequest(result.Errors.ToList());
        }
    }
}