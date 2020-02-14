using Autofac;
using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels.Cards
{
    public class OfflineModeCardViewModel : BaseViewModel
    {
        private readonly GenericRepository<AppSettings> appSettingsRepository = new GenericRepository<AppSettings>();
        private readonly GenericRepository<User> userRepository = new GenericRepository<User>();
        private readonly GenericRepository<Account> accountRepository = new GenericRepository<Account>();
        private readonly GenericRepository<Transaction> transactionRepository = new GenericRepository<Transaction>();
        private readonly ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        AppSettings offlineModeSetting;
        bool offlineModeEnabled;

        public OfflineModeCardViewModel()
        {
            ClearCacheCommand = new Command(ClearCache);
        }

        public event EventHandler OnClearSuccess;

        public ICommand ClearCacheCommand { get; }

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
                if(offlineModeSetting != null)
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
            await Task.Yield();
        }

        private async void ClearCache(object obj)
        {
            var all = localizationManager.Translate("All");
            var userData = localizationManager.Translate("UserData");
            var options = new string[] { all, userData };
            var action = await Application.Current.MainPage.DisplayActionSheet(localizationManager.Translate("WipeDataChoices"), "Cancel", null, options);

            if (action.Equals(all))
            {
                HandleSafeExecution(() =>
                {
                    userRepository.RemoveAll();
                    userRepository.Commit();

                    accountRepository.RemoveAll();
                    accountRepository.Commit();

                    transactionRepository.RemoveAll();
                    transactionRepository.Commit();

                    OnClearSuccess?.Invoke(this, EventArgs.Empty);
                });
            }

            if (action.Equals(userData))
            {
                HandleSafeExecution(() =>
                {
                    accountRepository.RemoveAll();
                    accountRepository.Commit();

                    transactionRepository.RemoveAll();
                    transactionRepository.Commit();

                    OnClearSuccess?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        private void SaveCloudSyncSetting()
        {
            if (!offlineModeEnabled)
            {
                ClearCache(null);
            }

            if(offlineModeSetting == null)
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

    }
}