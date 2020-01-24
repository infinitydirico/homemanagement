using HomeManagement.App.ViewModels.Cards;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguageCard : Frame
    {
        public LanguageCard()
        {
            InitializeComponent();
            BindingContext = new LanguageCardViewModel();
        }
    }
}