using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class SettingsViewModel : LocalizationBaseViewModel
    {
        CultureInfo[] cultures = new CultureInfo[] { new CultureInfo("es"), new CultureInfo("en") };
        private readonly GenericRepository<AppSettings> appSettingsRepository = new GenericRepository<AppSettings>();
        private readonly GenericRepository<User> userRepository = new GenericRepository<User>();
        private readonly GenericRepository<Account> accountRepository = new GenericRepository<Account>();
        private readonly GenericRepository<Charge> chargeRepository = new GenericRepository<Charge>();

        AppSettings offlineModeSetting;
        bool offlineModeEnabled;

        public SettingsViewModel()
        {
            ChangeLanguageCommand = new Command(ChangeLanguage);
            ClearCacheCommand = new Command(ClearCache);
        }

        public event EventHandler OnClearSuccess;

        public ICommand ChangeLanguageCommand { get; set; }

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
                offlineModeSetting.Enabled = offlineModeEnabled;
                OnPropertyChanged();

                SaveCloudSyncSetting();
            }
        }

        public void RefreshCaching()
        {
            HasCachedData = accountRepository.Any() || chargeRepository.Any();
            OnPropertyChanged(nameof(HasCachedData));
        }

        protected override async Task InitializeAsync()
        {
            offlineModeSetting = appSettingsRepository.FirstOrDefault(x => x.Name.Equals(AppSettings.GetOfflineModeSetting().Name));
            offlineModeEnabled = offlineModeSetting.Enabled;
            OnPropertyChanged(nameof(OfflineModeEnabled));
            await Task.Yield();
        }

        private async void ClearCache(object obj)
        {
            var options = new string[] { "All", "User Data" };
            var action = await Application.Current.MainPage.DisplayActionSheet("Choices", "Cancel", null, options);

            switch(action)
            {
                case "All":
                    HandleSafeExecution(() =>
                    {
                        userRepository.RemoveAll();
                        userRepository.Commit();

                        accountRepository.RemoveAll();
                        accountRepository.Commit();

                        chargeRepository.RemoveAll();
                        chargeRepository.Commit();

                        OnClearSuccess?.Invoke(this, EventArgs.Empty);
                    });
                    break;
                case "User Data":
                    HandleSafeExecution(() =>
                    {
                        accountRepository.RemoveAll();
                        accountRepository.Commit();

                        chargeRepository.RemoveAll();
                        chargeRepository.Commit();

                        OnClearSuccess?.Invoke(this, EventArgs.Empty);
                    });
                    break;
            }
        }

        private void ChangeLanguage()
        {
            var nextCulture = cultures.FirstOrDefault(x => !x.Name.Equals(localization.GetCurrentCulture().Name));
            localization.ChangeCulture(nextCulture);
        }

        private void SaveCloudSyncSetting()
        {
            if (!offlineModeEnabled)
            {
                ClearCache(null);
            }

            appSettingsRepository.Update(offlineModeSetting);
            appSettingsRepository.Commit();
        }
    }
}
