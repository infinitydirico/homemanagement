using System;
using System.Collections.Generic;
using System.Text;

namespace HomeManagement.App.ViewModels.Cards
{
    public class DailyBackupCardViewModel : BaseViewModel
    {
        private bool dailyBackupEnabled;

        public DailyBackupCardViewModel()
        {

        }

        public bool DailyBackupEnabled
        {
            get => dailyBackupEnabled;
            set
            {
                dailyBackupEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}
