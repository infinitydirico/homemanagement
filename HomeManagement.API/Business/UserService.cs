using HomeManagement.API.Data;
using HomeManagement.API.Data.Entities;
using HomeManagement.Business.Contracts;
using HomeManagement.Contracts;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
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
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ICryptography cryptography;
        private readonly IPreferenceService preferenceService;
        private readonly IUserSessionService userSessionService;
        private readonly ITransactionService transactionService;
        private readonly ICategoryService categoryService;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IApiRepositoryFactory apiRepositoryFactory;
        private readonly IUserMapper userMapper;

        public UserService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory,
            ICryptography cryptography,
            IPreferenceService preferenceService,
            IUserSessionService userSessionService,
            ITransactionService transactionService,
            ICategoryService categoryService,
            IRepositoryFactory repositoryFactory,
            IApiRepositoryFactory apiRepositoryFactory,
            IUserMapper userMapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.serviceScopeFactory = serviceScopeFactory;
            this.cryptography = cryptography;
            this.preferenceService = preferenceService;
            this.userSessionService = userSessionService;
            this.transactionService = transactionService;
            this.categoryService = categoryService;
            this.repositoryFactory = repositoryFactory;
            this.userMapper = userMapper;
            this.apiRepositoryFactory = apiRepositoryFactory;
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

                var language = user.Language ?? "en";

                using (var userRepository = repositoryFactory.CreateUserRepository())
                using (var accountRepository = repositoryFactory.CreateAccountRepository())
                using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
                using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
                {
                    userRepository.Add(userEntity);

                    accountRepository.Add(new Account
                    {
                        UserId = userEntity.Id,
                        Name = language.Contains("en") ? "Cash" : "Efectivo",
                        AccountType = Domain.AccountType.Cash,
                        CurrencyId = 1
                    });

                    accountRepository.Commit();

                    var categories = CategoryInitializer.GetDefaultCategories().Select(x => new Category
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Icon = x.Icon,
                        IsActive = true,
                        Measurable = true,
                        UserId = userEntity.Id
                    }).ToList();

                    categoryRepository.Add(categories);
                    categoryRepository.Commit();

                    preferencesRepository.Add(new Preferences
                    {
                        UserId = userEntity.Id,
                        Key = "PreferredCurrency",
                        Value = "USD",
                    });
                    preferencesRepository.Add(new Preferences
                    {
                        UserId = userEntity.Id,
                        Key = "Language",
                        Value = language,
                    });
                    preferencesRepository.Commit();
                }
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
            using (var userRepository = repositoryFactory.CreateUserRepository())
            using (var tokenRepository = apiRepositoryFactory.CreateTokenRepository())
            {
                string tokenValue = string.Empty;

                var user = userRepository.FirstOrDefault(x => x.Id.Equals(userId));

                if (tokenRepository.UserHasToken(appUserId))
                {
                    var dbToken = tokenRepository.FirstOrDefault(x => x.UserId.Equals(appUserId));

                    tokenRepository.Remove(appUserId);
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
            var accountRepository = repositoryFactory.CreateAccountRepository();

            var user = userSessionService.GetAuthenticatedUser();

            var accounts = accountRepository.Where(x => x.UserId.Equals(user.Id));

            var stream = new MemoryStream();
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Update, true))
            {
                foreach (var account in accounts)
                {
                    var entry = zip.CreateEntry($"{account.Name}.csv");
                    var file = transactionService.Export(account.Id);

                    using (var entryStream = entry.Open())
                    {
                        var st = new MemoryStream(file.Contents);

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
            using (var userRepository = repositoryFactory.CreateUserRepository())
            using (var tokenRepository = apiRepositoryFactory.CreateTokenRepository())
            using (var currencyRepository = repositoryFactory.CreateCurrencyRepository())
            {
                var password = cryptography.Decrypt(userModel.Password);

                var result = await signInManager.PasswordSignInAsync(userModel.Email, password, true, false);

                if (!result.Succeeded) return null;

                var appUser = await userManager.FindByEmailAsync(userModel.Email);

                var userEntity = userRepository.FirstOrDefault(x => x.Email.Equals(userModel.Email));

                userSessionService.RegisterScopedUser(userEntity.Email);

                var language = preferenceService.GetUserLanguage(userEntity.Id);
                var preferredCurrency = preferenceService.GetPreferredCurrency();

                var token = RenewToken(appUser.Id, userEntity.Id);

                return new UserModel
                {
                    Id = userEntity.Id,
                    Email = userEntity.Email,
                    Token = token,
                    Language = language,
                    Currency = preferredCurrency.Name
                };
            }
        }

        public async Task SignOut(UserModel userModel)
        {
            await signInManager.SignOutAsync();

            var appUser = await userManager.FindByEmailAsync(userModel.Email);
            using (var tokenRepository = apiRepositoryFactory.CreateTokenRepository())
            {
                if (tokenRepository.UserHasToken(appUser.Id)) tokenRepository.Remove(appUser.Id);
            }
        }

        public async Task DeleteUser(int userId)
        {
            using (var userRepository = repositoryFactory.CreateUserRepository())
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            using (var reminderRepository = repositoryFactory.CreateReminderRepository())
            using (var notificationRepository = repositoryFactory.CreateNotificationRepository())
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

                accountRepository.Commit();

                var reminders = reminderRepository.Where(x => x.UserId.Equals(user.Id));

                foreach (var reminder in reminders)
                {
                    var notifications = notificationRepository.Where(x => x.ReminderId.Equals(reminder.Id));

                    foreach (var notification in notifications)
                    {
                        notificationRepository.Remove(notification);
                    }

                    reminderRepository.Remove(reminder);
                    accountRepository.Commit();
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
        }

        public IEnumerable<UserModel> GetUsers()
        {
            using (var userRepository = repositoryFactory.CreateUserRepository())
            {
                return userRepository
                .GetAll()
                .Select(x => userMapper.ToModel(x))
                .ToList();
            }
        }

        public async Task<OperationResult> ChangePassword(string currentPassword, string newPassword)
        {
            var user = userSessionService.GetAuthenticatedUser();
            var appUser = await userManager.FindByEmailAsync(user.Email);
            var result = await userManager.ChangePasswordAsync(appUser, currentPassword, newPassword);
            return new OperationResult
            {
                Result = result.Succeeded ? Result.Succeed : Result.Error,
                Errors = result.Errors.Select(x => x.Description).ToList()
            };
        }

        public OperationResult CreateDefaultData(UserModel userModel)
        {
            using (var userRepository = repositoryFactory.CreateUserRepository())
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            {
                var language = userModel.Language ?? "en";

                var user = new User
                {
                    Email = userModel.Email
                };

                userRepository.Add(user);

                accountRepository.Add(new Account
                {
                    UserId = user.Id,
                    Name = language.Contains("en") ? "Cash" : "Efectivo",
                    AccountType = Domain.AccountType.Cash,
                    CurrencyId = 1
                });

                accountRepository.Commit();

                var categories = CategoryInitializer.GetDefaultCategories().Select(x => new Category
                {
                    Id = x.Id,
                    Name = x.Name,
                    Icon = x.Icon,
                    IsActive = true,
                    Measurable = true,
                    UserId = user.Id
                }).ToList();

                categoryRepository.Add(categories);
                categoryRepository.Commit();

                preferencesRepository.Add(new Preferences
                {
                    UserId = user.Id,
                    Key = "PreferredCurrency",
                    Value = "USD",
                });
                preferencesRepository.Add(new Preferences
                {
                    UserId = user.Id,
                    Key = "Language",
                    Value = language,
                });
                preferencesRepository.Commit();
            }
            return OperationResult.Succeed();
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

        Task<OperationResult> ChangePassword(string currentPassword, string newPassword);

        OperationResult CreateDefaultData(UserModel userModel);
    }
}
