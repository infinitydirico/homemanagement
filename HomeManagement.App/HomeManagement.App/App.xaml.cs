using Autofac;
using Autofac.Core;
using HomeManagement.App.Data;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using HomeManagement.App.Views.Login;
using HomeManagement.Contracts;
using HomeManagement.Core.Caching;
using HomeManagement.Core.Cryptography;
using HomeManagement.Data;
using HomeManagement.Localization;
using HomeManagement.Mapper;
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

            RegisterMappers(builder);

            RegisterRepositories(builder);

            RegisterManagers(builder);

            _container = builder.Build();
        }

        private void RegisterManagers(ContainerBuilder builder)
        {
            builder.RegisterType<ChargeManager>().As<IChargeManager>();
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<MobileAppLayerContext>().As<IPlatformContext>();

            builder
                .RegisterType<UserRepository>()
                .As<IUserRepository>()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(IPlatformContext) && pi.Name.Equals("platformContext"),
                        (pi, ctx) => _container.Resolve<IPlatformContext>()
                     )
                 );
        }

        private void RegisterMappers(ContainerBuilder builder)
        {
            builder.RegisterType<AccountMapper>().As<IAccountMapper>();
            builder.RegisterType<CategoryMapper>().As<ICategoryMapper>();
            builder.RegisterType<ChargeMapper>().As<IChargeMapper>();
            builder.RegisterType<UserMapper>().As<IUserMapper>();
        }

        private void RegisterServiceClients(ContainerBuilder builder)
        {
            builder.RegisterType<AccountServiceClient>().As<IAccountServiceClient>();
            builder.RegisterType<AuthServiceClient>().As<IAuthServiceClient>().SingleInstance();
            builder.RegisterType<ChargeServiceClient>().As<IChargeServiceClient>();
            builder.RegisterType<AccountMetricsServiceClient>().As<IAccountMetricsServiceClient>();
            builder.RegisterType<CategoryServiceClient>().As<ICategoryServiceClient>();
        }
    }
}
