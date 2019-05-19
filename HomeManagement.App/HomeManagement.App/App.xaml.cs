using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using HomeManagement.App.Views.Login;
using HomeManagement.Contracts;
using HomeManagement.Core.Caching;
using HomeManagement.Core.Cryptography;
using HomeManagement.Localization;
using Plugin.Connectivity;
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

            MainPage = new NavigationPage(new LoginPage());

            CrossConnectivity.Current.ConnectivityTypeChanged += (sender, args) =>
            {
                if(!args.IsConnected)
                {
                    MainPage.DisplayAlert("Info", "Lost internet connection", "Ok");
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
    }
}
