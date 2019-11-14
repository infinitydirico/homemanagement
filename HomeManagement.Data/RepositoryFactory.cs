using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IPlatformContext platformContext;
        protected readonly DbContext context;

        public RepositoryFactory(IPlatformContext platformContext)
        {
            this.platformContext = platformContext;
            context = platformContext.CreateDbContext();
        }

        public IAccountRepository CreateAccountRepository() => new AccountRepository(context);

        public ICategoryRepository CreateCategoryRepository() => new CategoryRepository(context);

        public IConfigurationSettingsRepository CreateConfigurationSettingsRepository() => new ConfigurationSettingsRepository(context);

        public ICurrencyRepository CreateCurrencyRepository() => new CurrencyRepository(context);

        public IMonthlyExpenseRepository CreateMonthlyExpenseRepository() => new MonthlyExpenseRepository(context);

        public INotificationRepository CreateNotificationRepository() => new NotificationRepository(context);

        public IPreferencesRepository CreatePreferencesRepository() => new PreferencesRepository(context);

        public IReminderRepository CreateReminderRepository() => new ReminderRepository(context);

        public IStorageItemRepository CreateStorageItemRepository() => new StorageItemRepository(context);

        public ITransactionRepository CreateTransactionRepository() => new TransactionRepository(context);

        public IUserCategoryRepository CreateUserCategoryRepository() => new UserCategoryRepository(context);

        public IUserRepository CreateUserRepository() => new UserRepository(context);
    }

    public interface IRepositoryFactory
    {
        IAccountRepository CreateAccountRepository();

        ITransactionRepository CreateTransactionRepository();

        ICategoryRepository CreateCategoryRepository();

        IConfigurationSettingsRepository CreateConfigurationSettingsRepository();

        IMonthlyExpenseRepository CreateMonthlyExpenseRepository();

        INotificationRepository CreateNotificationRepository();

        IPreferencesRepository CreatePreferencesRepository();

        ICurrencyRepository CreateCurrencyRepository();

        IReminderRepository CreateReminderRepository();

        IStorageItemRepository CreateStorageItemRepository();

        IUserCategoryRepository CreateUserCategoryRepository();

        IUserRepository CreateUserRepository();
    }
}
