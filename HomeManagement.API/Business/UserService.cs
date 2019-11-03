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
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using HomeManagement.Mapper;

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
        private readonly IUserSessionService userSessionService;
        private readonly ITransactionService transactionService;
        private readonly ICategoryService categoryService;
        private readonly IReminderRepository reminderRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IUserMapper userMapper;

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
            ITokenRepository tokenRepository,
            IUserSessionService userSessionService,
            ITransactionService transactionService,
            ICategoryService categoryService,
            IReminderRepository reminderRepository,
            INotificationRepository notificationRepository,
            IUserMapper userMapper)
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
            this.userSessionService = userSessionService;
            this.transactionService = transactionService;
            this.categoryService = categoryService;
            this.reminderRepository = reminderRepository;
            this.notificationRepository = notificationRepository;
            this.userMapper = userMapper;
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

                userRepository.Add(userEntity);                

                accountRepository.Add(new Account
                {
                    UserId = userEntity.Id,
                    Name = user.Language.Contains("en") ? "Cash" : "Efectivo",
                    AccountType = Domain.AccountType.Cash,
                    CurrencyId = 1
                });
                userRepository.Commit();

                userSessionService.RegisterScopedUser(applicationUser.Email);

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

        public MemoryStream DownloadUserData()
        {
            var user = userSessionService.GetAuthenticatedUser();

            var accounts = accountRepository.All.Where(x => x.UserId.Equals(user.Id));

            var stream = new MemoryStream();
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Update, true))
            {
                foreach (var account in accounts)
                {
                    var entry = zip.CreateEntry($"{account.Name}.csv");
                    var file = transactionService.Export(account.Id);

                    using (var entryStream = entry.Open())
                    {
                        var st = new MemoryStream(file.Content);

                        st.CopyTo(entryStream);
                    }
                }

                var categoryEntry = zip.CreateEntry("categories.csv");
                var categoryFile = categoryService.Export();

                using (var entryStream = categoryEntry.Open())
                {
                    var st = new MemoryStream(categoryFile.Contents);

                    st.CopyTo(entryStream);
                }
            }
            stream.Position = 0;
            return stream;
        }

        public async Task<UserModel> SignIn(UserModel userModel)
        {
            var password = cryptography.Decrypt(userModel.Password);

            var result = await signInManager.PasswordSignInAsync(userModel.Email, password, true, false);

            if (!result.Succeeded) return null;

            var appUser = await userManager.FindByEmailAsync(userModel.Email);

            var userEntity = userRepository.FirstOrDefault(x => x.Email.Equals(userModel.Email));

            var token = RenewToken(appUser.Id, userEntity.Id);

            userSessionService.RegisterScopedUser(userEntity.Email);

            return new UserModel
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Token = token,
                Language = preferenceService.GetUserLanguage(userEntity.Id),
                Currency = preferenceService.GetPreferredCurrency().Name
            };
        }

        public async Task SignOut(UserModel userModel)
        {
            await signInManager.SignOutAsync();

            var appUser = await userManager.FindByEmailAsync(userModel.Email);

            if (tokenRepository.UserHasToken(appUser.Id)) tokenRepository.Remove(appUser.Id);
        }

        public async Task DeleteUser(int userId)
        {
            var user = userRepository.GetById(userId);

            var appUser = await userManager.FindByEmailAsync(user.Email);

            var userAccounts = accountRepository.Where(x => x.UserId.Equals(user.Id));

            foreach (var account in userAccounts)
            {
                var transactions = transactionRepository.Where(x => x.AccountId.Equals(account.Id));

                foreach (var transaction in transactions)
                {
                    transactionRepository.Remove(transaction);
                }

                accountRepository.Remove(account);
                accountRepository.Commit();
            }

            var userCategories = userCategoryRepository.Where(x => x.UserId.Equals(user.Id));

            foreach (var userCategory in userCategories)
            {
                categoryRepository.Remove(userCategory.CategoryId, user);
            }

            userCategoryRepository.Commit();

            var reminders = reminderRepository.Where(x => x.UserId.Equals(user.Id));

            foreach (var reminder in reminders)
            {
                var notifications = notificationRepository.Where(x => x.ReminderId.Equals(reminder.Id));

                foreach (var notification in notifications)
                {
                    notificationRepository.Remove(notification);
                }

                reminderRepository.Remove(reminder);
                reminderRepository.Commit();
            }

            var userPreferences = preferencesRepository.Where(x => x.UserId.Equals(user.Id));

            foreach (var userPreference in userPreferences)
            {
                preferencesRepository.Remove(userPreference);
            }
            preferencesRepository.Commit();

            userRepository.Remove(user.Id);
            userRepository.Commit();

            await userManager.DeleteAsync(appUser);
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return userRepository
                .GetAll()
                .Select(x => userMapper.ToModel(x))
                .ToList();
        }
    }

    public interface IUserService
    {
        Task<OperationResult> CreateUser(UserModel user);

        Task<UserModel> SignIn(UserModel userModel);

        Task SignOut(UserModel userModel);

        string RenewToken(string appUserId, int userId);

        MemoryStream DownloadUserData();

        Task DeleteUser(int userId);

        IEnumerable<UserModel> GetUsers();
    }
}
