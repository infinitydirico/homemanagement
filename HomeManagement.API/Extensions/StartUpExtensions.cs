using HomeManagement.API.Business;
using HomeManagement.API.Data;
using HomeManagement.API.Data.Repositories;
using HomeManagement.API.Exportation;
using HomeManagement.API.Throttle;
using HomeManagement.Contracts;
using HomeManagement.Core.Cryptography;
using HomeManagement.Data;
using HomeManagement.Mapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HomeManagement.API.Extensions
{
    public static class StartUpExtensions
    {
        public static void AddMiddleware(this IServiceCollection services)
        {
            services.AddScoped<IThrottleCore, ThrottleCore>();

            services.AddScoped<ICryptography, AesCryptographyService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPlatformContext, WebAppLayerContext>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<ICategoryRepository, API.Data.Repositories.CategoryRepository>();

            services.AddScoped<IUserCategoryRepository, UserCategoryRepository>();

            services.AddScoped<IReminderRepository, ReminderRepository>();

            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IPreferencesRepository, PreferencesRepository>();
            
            services.AddScoped<ITokenRepository, TokenRepository>();

            services.AddScoped<IWebClientRepository, WebClientRepository>();

            services.AddScoped<IDataLogRepository, DataLogRepository>();

            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            services.AddScoped<IStorageItemRepository, StorageItemRepository>();
            
            //with the throttle filter with persisted repo, the requests take around 100ms to respond
            //with memory values, it takes 30ms
            //services.AddScoped<IWebClientRepository, MemoryWebClientRepository>();            
        }

        public static void AddMappers(this IServiceCollection services)
        {
            services.AddScoped<ICategoryMapper, CategoryMapper>();

            services.AddScoped<IAccountMapper, AccountMapper>();

            services.AddScoped<IUserMapper, UserMapper>();

            services.AddScoped<ITransactionMapper, TransactionMapper>();

            services.AddScoped<IReminderMapper, ReminderMapper>();

            services.AddScoped<INotificationMapper, NotificationMapper>();

            services.AddScoped<ICurrencyMapper, CurrencyMapper>();

            services.AddScoped<IStorageItemMapper, StorageItemMapper>();
        }

        public static void AddExportableComponents(this IServiceCollection services)
        {
            services.AddScoped<IExportableCategory, ExportableCategory>();

            services.AddScoped<IExportableTransaction, ExportableTransaction>();
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPreferenceService, PreferenceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<IMetricsService, MetricsService>();
        }

        public static CorsPolicy BuildCorsPolicy(this CorsOptions corsOptions)
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            return corsBuilder.Build();
        }
    }
}
