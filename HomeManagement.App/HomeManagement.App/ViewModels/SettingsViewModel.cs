using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
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

        AppSettings coudSyncSetting;
        bool coudSyncEnabled;

        public SettingsViewModel()
        {
            ChangeLanguageCommand = new Command(ChangeLanguage);
            ClearCacheCommand = new Command(ClearCache);
        }

        protected override async Task InitializeAsync()
        {
            coudSyncSetting = appSettingsRepository.FirstOrDefault(x => x.Name.Equals(AppSettings.GetCloudSyncSetting().Name));
            coudSyncEnabled = coudSyncSetting.Enabled;
            OnPropertyChanged(nameof(CoudSyncEnabled));
            await Task.Yield();
        }

        public event EventHandler OnClearSuccess;

        public ICommand ChangeLanguageCommand { get; set; }

        public ICommand ClearCacheCommand { get; }

        public string ChangeLanguageText { get; set; } = "ChangeLanguage";

        public string CloudSyncText { get; set; } = "CloudSync";

        public string CloudSyncStatusText { get; private set; } = "Disabled";

        public string LanguageTitleText { get; set; } = "LanguageOptions";

        public string SyncTitleText { get; set; } = "SyncOptions";

        public bool CoudSyncEnabled
        {
            get
            {
                return coudSyncEnabled;
            }
            set
            {
                coudSyncEnabled = value;
                coudSyncSetting.Enabled = coudSyncEnabled;
                CloudSyncStatusText = GetCloudSyncLabel(coudSyncEnabled);

                OnPropertyChanged();
                OnPropertyChanged(nameof(CloudSyncStatusText));

                SaveCloudSyncSetting();
            }
        }

        private void ClearCache(object obj)
        {
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
        }

        private void ChangeLanguage()
        {
            var nextCulture = cultures.FirstOrDefault(x => !x.Name.Equals(localization.GetCurrentCulture().Name));
            localization.ChangeCulture(nextCulture);
        }

        private void SaveCloudSyncSetting()
        {
            if (!coudSyncEnabled)
            {
                ClearCache(null);
            }

            appSettingsRepository.Update(coudSyncSetting);
            appSettingsRepository.Commit();
        }

        private string GetCloudSyncLabel(bool sync) => sync ? "Enabled" : "Disabled";
    }
}
