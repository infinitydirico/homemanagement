using HomeManagement.API.Data.Entities;
using HomeManagement.API.Data.Repositories;
using HomeManagement.Contracts;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeManagement.API.Business
{
    public class UserService : IUserService
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
        private readonly IUserCategoryRepository userCategoryRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IPreferenceService preferenceService;
        private readonly ITokenRepository tokenRepository;

        private User user;

        public UserService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IAccountRepository accountRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IServiceScopeFactory serviceScopeFactory,
            IPreferencesRepository preferencesRepository,
            ICryptography cryptography,
            IUserCategoryRepository userCategoryRepository,
            ITransactionRepository transactionRepository,
            IPreferenceService preferenceService,
            ITokenRepository tokenRepository)
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
            this.userCategoryRepository = userCategoryRepository;
            this.transactionRepository = transactionRepository;
            this.preferenceService = preferenceService;
            this.tokenRepository = tokenRepository;
        }

        public async Task<OperationResult> CreateUser(UserModel user)
        {
            var result = await userManager.CreateAsync(new ApplicationUser
            {
                Email = user.Email,
                UserName = user.Email
            }, cryptography.Decrypt(user.Password));

            if (result.Succeeded)
            {
                var applicationUser = await userManager.FindByEmailAsync(user.Email);

                var userEntity = new User
                {
                    Email = applicationUser.Email,
                };

                await userRepository.AddAsync(userEntity);

                accountRepository.Add(new Account
                {
                    UserId = userEntity.Id,
                    Name = user.Language.Contains("en") ? "Cash" : "Efectivo",
                    AccountType = Domain.AccountType.Cash,
                    CurrencyId = 1
                });

                preferenceService.ChangeLanguage(user.Language);

                var currencies = preferenceService.GetCurrencies();
                preferenceService.ChangeCurrency(currencies.First(x => x.Name.Equals("USD")));

                accountRepository.Commit();

                return OperationResult.Succeed();
            }
            else
            {
                return new OperationResult
                {
                    Result = Result.Error,
                    Errors = result.Errors.Select(x => x.Description).ToList()
                };
            }
        }

        public string RenewToken(string appUserId, int userId)
        {
            string tokenValue = string.Empty;

            var user = userRepository.FirstOrDefault(x => x.Id.Equals(userId));

            if (tokenRepository.UserHasToken(appUserId))
            {
                var dbToken = tokenRepository.FirstOrDefault(x => x.UserId.Equals(appUserId));

                tokenRepository.Remove(appUserId);
                tokenRepository.Commit();
            }

            tokenValue = CreateToken(user.Email);

            tokenRepository.Add(new IdentityUserToken<string>
            {
                UserId = appUserId,
                LoginProvider = nameof(JwtSecurityToken),
                Name = nameof(JwtSecurityToken),
                Value = tokenValue
            });
            tokenRepository.Commit();

            return tokenValue;
        }

        private string CreateToken(string email)
        {
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Issuer"],
                   audience: configuration["Audience"],
                   claims: claims,
                   expires: DateTime.UtcNow.AddDays(1),
                   notBefore: DateTime.UtcNow,
                   signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SigningKey"])),
                        SecurityAlgorithms.HmacSha256)
            );

            return jwtSecurityToken.WriteToken(token);
        }

        public async Task<UserModel> SignIn(UserModel userModel)
        {
            var password = cryptography.Decrypt(userModel.Password);

            var result = await signInManager.PasswordSignInAsync(userModel.Email, password, true, false);

            if (!result.Succeeded) return null;

            var appUser = await userManager.FindByEmailAsync(userModel.Email);

            var userEntity = userRepository.FirstOrDefault(x => x.Email.Equals(userModel.Email));

            var token = RenewToken(appUser.Id, userEntity.Id);

            return new UserModel
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Token = token,
                Language = preferenceService.GetUserLanguage(userEntity.Id)
            };
        }

        public async Task SignOut(UserModel userModel)
        {
            await signInManager.SignOutAsync();

            var appUser = await userManager.FindByEmailAsync(userModel.Email);

            if (tokenRepository.UserHasToken(appUser.Id)) tokenRepository.Remove(appUser.Id);
        }
    }

    public interface IUserService
    {
        Task<OperationResult> CreateUser(UserModel user);

        Task<UserModel> SignIn(UserModel userModel);

        Task SignOut(UserModel userModel);

        string RenewToken(string appUserId, int userId);
    }
}
