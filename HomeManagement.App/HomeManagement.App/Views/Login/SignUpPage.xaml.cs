using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public SignUpPage()
        {
            InitializeComponent();

            var signUpVm = ((SignUpViewModel)BindingContext);

            signUpVm.OnError += SignUpVm_OnError;
            signUpVm.OnSuccess += SignUpVm_OnSuccess;

            Title = localizationManager.Translate("SignUp");
        }

        private void SignUpVm_OnSuccess(object sender, EventArgs e)
        {
            Navigation.RemovePage(this);
        }

        private void SignUpVm_OnError(object sender, Common.ErrorEventArgs e)
        {
            DisplayAlert("Error", e.ErrorMessage, "Ok");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            usernameEntry.Focus();
        }
    }
}