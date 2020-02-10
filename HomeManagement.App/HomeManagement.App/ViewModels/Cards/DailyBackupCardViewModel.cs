using HomeManagement.App.Services.Rest;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels.Cards
{
    public class DailyBackupCardViewModel : BaseViewModel
    {
        private readonly PreferenceServiceClient preferenceServiceClient = new PreferenceServiceClient();
        private bool dailyBackupEnabled;

        public bool DailyBackupEnabled
        {
            get => dailyBackupEnabled;
            set
            {
                dailyBackupEnabled = value;
                OnPropertyChanged();
            }
        }

        protected override async Task InitializeAsync()
        {
            var result = await preferenceServiceClient.GetEnableBackups();
            DailyBackupEnabled = result;
        }
    }
}
