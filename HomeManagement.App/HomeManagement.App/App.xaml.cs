using Autofac;
using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using HomeManagement.App.Views.Login;
using HomeManagement.App.Views.Main;
using HomeManagement.Contracts;
using HomeManagement.Core.Caching;
using HomeManagement.Core.Cryptography;
using HomeManagement.Localization;
using Plugin.Connectivity;
using System;
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

            var userRepository = new GenericRepository<User>();

            Page page = new LoginPage();

            var authManager = _container.Resolve<IAuthenticationManager>();
            if (authManager.HasValidCredentialsAvaible())
            {
                var user = authManager.GetStoredUser();
                authManager.AuthenticateAsync(user.Email, user.Password);

                page = new MainPage();

                NavigationPage.SetHasBackButton(page, false);

                NavigationPage.SetHasNavigationBar(page, true);
            }

            MainPage = new NavigationPage(page);

            CrossConnectivity.Current.ConnectivityTypeChanged += (sender, args) =>
            {
                if (!args.IsConnected)
                {
                    var p = new OfflinePage();
                    NavigationPage.SetHasBackButton(p, false);
                    MainPage.Navigation.PushAsync(p);
                }
            };
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
            builder.RegisterType<ChargeManager>().As<IChargeManager>();
            builder.RegisterType<AuthenticationManager>().As<IAuthenticationManager>();
            builder.RegisterType<AccountManager>().As<IAccountManager>();
            builder.RegisterType<MetricsManager>().As<IMetricsManager>();
            builder.RegisterType<CategoryManager>().As<ICategoryManager>();
            builder.RegisterType<LocalizationManager>().As<ILocalizationManager>();
        }

        private void RegisterServiceClients(ContainerBuilder builder)
        {
            builder.RegisterType<AccountServiceClient>().As<IAccountServiceClient>();
            builder.RegisterType<AuthServiceClient>().As<IAuthServiceClient>().SingleInstance();
            builder.RegisterType<ChargeServiceClient>().As<IChargeServiceClient>();
            builder.RegisterType<AccountMetricsServiceClient>().As<IAccountMetricsServiceClient>();
            builder.RegisterType<CategoryServiceClient>().As<ICategoryServiceClient>();
            builder.RegisterType<CurrencyServiceClient>().As<ICurrencyServiceClient>();
        }

        private void InitializeDefaultValues()
        {
            _container.Resolve<ICachingService>().Store("ForceApiCall", false);
        }
    }
}
