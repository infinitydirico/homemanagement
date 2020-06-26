
using HomeManagement.App.Common;
using HomeManagement.App.ViewModels.Cards;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TwoFactorAuthentication : Frame
    {
        TwoFactorAuthenticationViewModel viewModel = new TwoFactorAuthenticationViewModel();
        public TwoFactorAuthentication()
        {
            InitializeComponent();
            BindingContext = viewModel;
            viewModel.OnCodeChanging += OnCodeChanging;
        }

        private async void OnCodeChanging(object sender, CodeChangeEventArgs e)
        {
            if(e.HasChanged) await codeLabel.FadeTo(1, 250, Easing.BounceIn);
            else await codeLabel.FadeTo(0, 250, Easing.BounceOut);
        }
    }
}