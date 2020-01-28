using HomeManagement.App.ViewModels.Cards;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyBackupCard : Frame
    {
        public DailyBackupCard()
        {
            InitializeComponent();
            BindingContext = new DailyBackupCardViewModel();
        }
    }
}