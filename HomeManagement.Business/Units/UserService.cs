using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace HomeManagement.Business.Units
{
    public class UserService : IUserService
    {
        private readonly IExportableTransaction exportableTransaction;
        private readonly ICategoryService categoryService;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IUserMapper userMapper;

        public UserService(
            IExportableTransaction exportableTransaction,
            ICategoryService categoryService,
            IRepositoryFactory repositoryFactory,
            IUserMapper userMapper)
        {
            this.exportableTransaction = exportableTransaction;
            this.categoryService = categoryService;
            this.repositoryFactory = repositoryFactory;
            this.userMapper = userMapper;
        }

        public OperationResult CreateUser(string email, string language)
        {
            var userEntity = new User
            {
                Email = email,
            };

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

                var categories = CategoryRepository.GetDefaultCategories().Select(x => new Category
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

        public MemoryStream DownloadUserData(int userId)
        {
            var accountRepository = repositoryFactory.CreateAccountRepository();
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var accounts = accountRepository.Where(x => x.UserId.Equals(userId));

            var stream = new MemoryStream();
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Update, true))
            {
                foreach (var account in accounts)
                {
                    var entry = zip.CreateEntry($"{account.Name}.csv");

                    var transactions = transactionRepository.GetByAccount(account.Id).ToList();
                    var contents = exportableTransaction.ToCsv(transactions);

                    using (var entryStream = entry.Open())
                    {
                        var st = new MemoryStream(contents);

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

        public OperationResult DeleteUser(int userId)
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
            }

            return OperationResult.Succeed();
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
    }
}
