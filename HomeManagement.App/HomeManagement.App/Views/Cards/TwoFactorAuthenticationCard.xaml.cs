
using HomeManagement.App.Common;
using HomeManagement.App.ViewModels.Cards;
using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TwoFactorAuthentication : Frame
    {
        TwoFactorAuthenticationViewModel viewModel = new TwoFactorAuthenticationViewModel();
        private Timer timer;
        private double expiration;

        public TwoFactorAuthentication()
        {
            InitializeComponent();
            BindingContext = viewModel;
            viewModel.OnCodeChanging += OnCodeChanging;            
        }

        private async void OnCodeChanging(object sender, CodeChangeEventArgs e)
        {
            if(e.HasChanged)
            {
                await codeLabel.FadeTo(1, 250, Easing.BounceIn);

                if(timer == null)
                {
                    timer = new Timer(UpdateProgress, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
                }
            }
            else await codeLabel.FadeTo(0, 250, Easing.BounceOut);
        }

        private async void UpdateProgress(object state = null)
        {
            if (!viewModel.IsEnabled)
            {
                countdown.Progress = 0;
                return;
            }

            expiration = (viewModel.ExpirationDate - DateTime.Now).Seconds;
            var value = expiration / 30;
            
            await countdown.ProgressTo(value, 250, Easing.SinOut);
        }
    }
}