using HomeManagement.App.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();

            var signUpVm = ((SignUpViewModel)BindingContext);

            signUpVm.OnError += SignUpVm_OnError;
            signUpVm.OnSuccess += SignUpVm_OnSuccess;
        }

        private void SignUpVm_OnSuccess(object sender, EventArgs e)
        {
            Navigation.RemovePage(this);
        }

        private void SignUpVm_OnError(object sender, Common.ErrorEventArgs e)
        {
            DisplayAlert("Error", e.ErrorMessage, "Ok");
        }
    }
}