using Autofac;
using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services;
using HomeManagement.App.Services.BackgroundWorker;
using HomeManagement.App.Views.Main;
using HomeManagement.Contracts;
using HomeManagement.Core.Caching;
using HomeManagement.Core.Cryptography;
using HomeManagement.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            InitializeWorkers();

            MainPage = new MainPage();

            Connectivity.ConnectivityChanged += (s, e) =>
            {
                var appSettingsRepository = new GenericRepository<AppSettings>();
                var offlineModeSetting = appSettingsRepository.FirstOrDefault(x => x.Name.Equals(AppSettings.GetOfflineModeSetting().Name));

                if (offlineModeSetting != null && offlineModeSetting.Enabled) return;

                if (e.NetworkAccess.Equals(NetworkAccess.None))
                {
                    MainPage.Navigation.PushModalAsync(new OfflinePage());
                }
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) => Logger.LogException(e.ExceptionObject as Exception);
        }

        public static List<BaseWorker> Workers { get; set; }

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

            RegisterManagers(builder);

            _container = builder.Build();
        }

        private void RegisterManagers(ContainerBuilder builder)
        {
            builder.RegisterType<TransactionManager>().As<ITransactionManager>();
            builder.RegisterType<AuthenticationManager>().As<IAuthenticationManager>().SingleInstance();
            builder.RegisterType<AccountManager>().As<IAccountManager>();
            builder.RegisterType<MetricsManager>().As<IMetricsManager>();
            builder.RegisterType<CategoryManager>().As<ICategoryManager>();
            builder.RegisterType<LocalizationManager>().As<ILocalizationManager>();
            builder.RegisterType<NotificationManager>().As<INotificationManager>();
            builder.RegisterType<StorageManager>().As<IStorageManager>();            
        }

        private void InitializeDefaultValues()
        {
            _container.Resolve<ICachingService>().Store("ForceApiCall", false);
        }

        private void InitializeWorkers()
        {
            Workers = new List<BaseWorker>
            {
                new SincronizationWorker()
            };

            var authenticationManager = _container.Resolve<IAuthenticationManager>();
            if (authenticationManager.IsAuthenticated())
            {
                StartWorkers(null, EventArgs.Empty);
            }
            else
            {
                authenticationManager.OnAuthenticationChanged += StartWorkers;
            }            
        }

        private void StartWorkers(object sender, EventArgs e)
        {
            Parallel.ForEach(Workers, w =>
            {
                if(!w.Started) w.Start();
            });
        }
    }
}
