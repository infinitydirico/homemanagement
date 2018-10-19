using System;
using Autofac;
using Autofac.Core;
using HomeManagement.App.Data;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Components;
using HomeManagement.App.Services.Rest;
using HomeManagement.App.Views.Login;
using HomeManagement.Contracts;
using HomeManagement.Contracts.Mapper;
using HomeManagement.Contracts.Repositories;
using HomeManagement.Core.Cryptography;
using HomeManagement.Data;
using HomeManagement.Localization;
using HomeManagement.Mapper;
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

            builder.RegisterType<ApplicationValues>().As<IApplicationValues>().SingleInstance();
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


        //private void InitializeDependencies()
        //{
        //    RegisterServiceClients();

        //    RegisterMappers();

        //    RegisterRepositories();

        //    DependencyService.Register<IMetadataHandler, MetadataHandler>();
        //    DependencyService.Register<ILanguageFactory, LanguageFactory>();
        //    DependencyService.Register<ICryptography, AesCryptographyService>();
        //}

        //private void RegisterServiceClients()
        //{
        //    DependencyService.Register<IAccountServiceClient, AccountServiceClient>();
        //    DependencyService.Register<IAuthServiceClient, AuthServiceClient>();
        //    DependencyService.Register<IChargeServiceClient, ChargeServiceClient>();
        //    DependencyService.Register<IAccountMetricsServiceClient, AccountMetricsServiceClient>();
        //    DependencyService.Register<ICategoryServiceClient, CategoryServiceClient>();
        //}

        //private void RegisterMappers()
        //{
        //    DependencyService.Register<IAccountMapper, AccountMapper>();
        //    DependencyService.Register<ICategoryMapper, CategoryMapper>();
        //    DependencyService.Register<IChargeMapper, ChargeMapper>();
        //    DependencyService.Register<IUserMapper, UserMapper>();
        //}

        //private void RegisterRepositories()
        //{
        //    DependencyService.Register<IPlatformContext, MobileAppLayerContext>();

        //    //DependencyService.Register<IUserRepository, UserRepository>();

        //}
    }
}
