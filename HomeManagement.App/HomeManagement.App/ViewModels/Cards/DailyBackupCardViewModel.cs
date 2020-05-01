using HomeManagement.App.Services.Rest;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels.Cards
{
    public class DailyBackupCardViewModel : BaseViewModel
    {
        private readonly PreferenceServiceClient preferenceServiceClient = new PreferenceServiceClient();
        private readonly TransactionServiceClient transactionServiceClient = new TransactionServiceClient();
        private bool dailyBackupEnabled;

        public DailyBackupCardViewModel()
        {
            DonwloadUserDataCommand = new Command(DownloadUserData);
        }

        public ICommand DonwloadUserDataCommand { get; }

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

        private void DownloadUserData(object obj)
        {
            Task.Run(async () =>
            {
                var stream = await transactionServiceClient.DownloadUserData();

                string filename = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "userdata.rar");

                using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filename)
                });
            });
        }
    }
}
