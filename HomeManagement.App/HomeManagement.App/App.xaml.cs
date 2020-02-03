using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.Services;
using HomeManagement.App.Services.Rest;
using HomeManagement.App.Views.Login;
using HomeManagement.App.Views.Main;
using HomeManagement.Contracts;
using HomeManagement.Core.Caching;
using HomeManagement.Core.Cryptography;
using HomeManagement.Localization;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HomeManagement.App
{
    public partial class App : Application
    {
        public static IContainer _container;

        public App()
        {
            InitializeComponent();

            InitializeDependencies();

            InitializeDefaultValues();
            
            var authManager = _container.Resolve<IAuthenticationManager>();
            if (authManager.HasValidCredentialsAvaible())
            {
                var user = authManager.GetStoredUser();
                authManager.AuthenticateAsync(user.Email, user.Password);

                var page = new DashboardPage();

                NavigationPage.SetHasBackButton(page, false);

                NavigationPage.SetHasNavigationBar(page, true);

                MainPage = new NavigationPage(page);
            }
            else
            {
                Page page = new LoginPage();

                MainPage = new NavigationPage(page);
            }

            Connectivity.ConnectivityChanged += (s, e) =>
            {
                if (e.NetworkAccess.Equals(NetworkAccess.None))
                {
                    var p = new OfflinePage();
                    NavigationPage.SetHasBackButton(p, false);
                    MainPage.Navigation.PushAsync(p);
                }
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) => Logger.LogException(e.ExceptionObject as Exception);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void InitializeDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MemoryCache>().As<ICachingService>().SingleInstance();
            builder.RegisterType<AesCryptographyService>().As<ICryptography>();

            builder.RegisterType<LocalizationLanguage>().As<ILocalization>().SingleInstance();

            RegisterServiceClients(builder);

            RegisterManagers(builder);

            _container = builder.Build();
        }

        private void RegisterManagers(ContainerBuilder builder)
        {
            builder.RegisterType<TransactionManager>().As<ITransactionManager>();
            builder.RegisterType<AuthenticationManager>().As<IAuthenticationManager>();
            builder.RegisterType<AccountManager>().As<IAccountManager>();
            builder.RegisterType<MetricsManager>().As<IMetricsManager>();
            builder.RegisterType<CategoryManager>().As<ICategoryManager>();
            builder.RegisterType<LocalizationManager>().As<ILocalizationManager>();
            builder.RegisterType<NotificationManager>().As<INotificationManager>();
            builder.RegisterType<StorageManager>().As<IStorageManager>();            
        }

        private void RegisterServiceClients(ContainerBuilder builder)
        {
            builder.RegisterType<AccountServiceClient>().As<IAccountServiceClient>();
            builder.RegisterType<AuthServiceClient>().As<IAuthServiceClient>().SingleInstance();
            builder.RegisterType<TransactionServiceClient>().As<ITransactionServiceClient>();
            builder.RegisterType<AccountMetricsServiceClient>().As<IAccountMetricsServiceClient>();
            builder.RegisterType<CategoryServiceClient>().As<ICategoryServiceClient>();
            builder.RegisterType<CurrencyServiceClient>().As<ICurrencyServiceClient>();
            builder.RegisterType<NotificationServiceClient>().As<INotificationServiceClient>();
            builder.RegisterType<PreferenceServiceClient>().As<IPreferenceServiceClient>();
        }

        private void InitializeDefaultValues()
        {
            _container.Resolve<ICachingService>().Store("ForceApiCall", false);
        }
    }
}
