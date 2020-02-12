
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Views.AccountPages;
using HomeManagement.App.Views.Transactions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Resources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountsDataTemplate : ResourceDictionary
    {
        public AccountsDataTemplate()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            Rotate(stackLayout);
        }

        private async void Rotate(StackLayout parent)
        {
            var layouts = parent.Children.ToList();

            var visibleLayout = layouts.Last(x => x.IsVisible);
            var hiddenLayout = layouts.Last(x => !x.IsVisible);

            if (hiddenLayout.IsVisible) await RotateViews(hiddenLayout, visibleLayout);
            else await RotateViews(visibleLayout, hiddenLayout);
        }

        private async Task RotateViews(View source, View target)
        {
            await source.RotateXTo(-90, 250, Easing.SpringIn);

            source.IsVisible = false;
            target.IsVisible = true;

            target.RotationX = -90;
            await target.RotateXTo(0, 250, Easing.SpringOut);
        }

        private void ViewTransactionsList(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent.Parent as StackLayout;
            Application.Current.MainPage.Navigation.PushAsync(new TransactionListPage(stacklayout.BindingContext as Account));
        }

        private void Edit(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent.Parent as StackLayout;
            var account = stacklayout.BindingContext as Account;

            var editPage = new EditAccountPage(account);

            Application.Current.MainPage.Navigation.PushAsync(editPage);
        }
    }
}