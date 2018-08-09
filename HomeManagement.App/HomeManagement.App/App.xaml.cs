using System;
using HomeManagement.App.Services.Components;
using HomeManagement.App.Services.Components.Language;
using HomeManagement.App.Services.Rest;
using HomeManagement.App.Views.Login;
using HomeManagement.Contracts.Mapper;
using HomeManagement.Mapper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HomeManagement.App
{
    public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            InitializeDependencies();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        private void InitializeDependencies()
        {
            RegisterServiceClients();

            RegisterMappers();

            DependencyService.Register<IMetadataHandler, MetadataHandler>();
            DependencyService.Register<ILanguageFactory, LanguageFactory>();

        }

        private void RegisterServiceClients()
        {
            DependencyService.Register<IAccountServiceClient, AccountServiceClient>();
            DependencyService.Register<IAuthServiceClient, AuthServiceClient>();
            DependencyService.Register<IChargeServiceClient, ChargeServiceClient>();
            DependencyService.Register<IAccountMetricsServiceClient, AccountMetricsServiceClient>();
            DependencyService.Register<ICategoryServiceClient, CategoryServiceClient>();
        }

        private void RegisterMappers()
        {
            DependencyService.Register<IAccountMapper, AccountMapper>();
            DependencyService.Register<ICategoryMapper, CategoryMapper>();
            DependencyService.Register<IChargeMapper, ChargeMapper>();
            DependencyService.Register<IUserMapper, UserMapper>();
        }
    }
}
