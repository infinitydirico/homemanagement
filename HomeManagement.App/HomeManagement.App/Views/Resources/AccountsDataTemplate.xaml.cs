
using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Extensions;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
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
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        Account account;

        public AccountsDataTemplate()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            account = stackLayout.BindingContext as Account;
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
            Application.Current.MainPage.Navigation.PushAsync(new TransactionListPage(account));
        }

        private void Edit(object sender, EventArgs e)
        {
            var editPage = new EditAccountPage(account);

            MessagingCenter.Subscribe<EditAccountPage>(editPage, Constants.Messages.UpdateOnAppearing, p =>
            {
                var viewModel = ((Button)sender).Parent.GetViewModel<AccountsViewModel>();
                viewModel.Refresh(); 
                MessagingCenter.Unsubscribe<EditAccountPage>(p, Constants.Messages.UpdateOnAppearing);
            });

            Application.Current.MainPage.Navigation.PushAsync(editPage);
        }

        private async void Delete(object sender, EventArgs e)
        {
            var result = await Application.Current.MainPage.DisplayAlert(localizationManager.Translate("Warning"),
                                                                        $"{localizationManager.Translate("DeleteAccount")} {account.Name}",
                                                                        "Ok",
                                                                        localizationManager.Translate("Cancel"));
            if (result)
            {
                var viewModel = ((Button)sender).Parent.GetViewModel<AccountsViewModel>();
                await viewModel.Delete(account);
                viewModel.Refresh();
            }
        }
    }
}