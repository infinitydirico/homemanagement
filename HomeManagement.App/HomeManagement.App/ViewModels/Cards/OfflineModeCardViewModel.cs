using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.BackgroundWorker;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomeManagement.App.ViewModels.Cards
{
    public class OfflineModeCardViewModel : BaseViewModel
    {
        private readonly GenericRepository<AppSettings> appSettingsRepository = new GenericRepository<AppSettings>();
        private readonly GenericRepository<Account> accountRepository = new GenericRepository<Account>();
        private readonly GenericRepository<Transaction> transactionRepository = new GenericRepository<Transaction>();

        AppSettings offlineModeSetting;
        bool offlineModeEnabled;

        public bool HasCachedData { get; private set; }

        public bool OfflineModeEnabled
        {
            get
            {
                return offlineModeEnabled;
            }
            set
            {
                offlineModeEnabled = value;
                if (offlineModeSetting != null)
                {
                    offlineModeSetting.Enabled = offlineModeEnabled;
                }
                OnPropertyChanged();

                SaveCloudSyncSetting();
            }
        }

        protected override async Task InitializeAsync()
        {
            offlineModeSetting = appSettingsRepository.FirstOrDefault(x => x.Name.Equals(AppSettings.GetOfflineModeSetting().Name));
            offlineModeEnabled = offlineModeSetting?.Enabled ?? false;
            OnPropertyChanged(nameof(OfflineModeEnabled));
            await Task.Delay(250);
        }

        private void ClearLocalCache(object obj = null)
        {
            Task.Run(() =>
            {
                HandleSafeExecution(() =>
                {
                    accountRepository.RemoveAll();
                    accountRepository.Commit();

                    transactionRepository.RemoveAll();
                    transactionRepository.Commit();

                    Preferences.Set("FullSync", true);
                });
            });
        }

        private void SaveCloudSyncSetting()
        {
            if (initializing) return;

            if (!offlineModeEnabled) ClearLocalCache();
            else RunSincronization();

            if (offlineModeSetting == null)
            {
                offlineModeSetting = AppSettings.GetOfflineModeSetting();
                offlineModeSetting.Enabled = offlineModeEnabled;
                appSettingsRepository.Add(offlineModeSetting);
            }
            else
            {
                appSettingsRepository.Update(offlineModeSetting);
            }

            appSettingsRepository.Commit();
        }

        private void RunSincronization()
        {
            Task.Run(async () =>
            {
                var syncWorker = App.Workers.First() as SincronizationWorker;
                await syncWorker.NeedsSincronization();
                syncWorker.RunWork(null);
            });
        }
    }
}