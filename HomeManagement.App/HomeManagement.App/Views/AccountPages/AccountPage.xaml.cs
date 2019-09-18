using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Charges;
using HomeManagement.App.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.AccountPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        AccountsViewModel viewModel = new AccountsViewModel();
        Modal modal;
        int offsetStart = 55;
        int offsetEnd = 110;

        public AccountPage()
        {
            InitializeComponent();

            modal = new Modal(this);
            BindingContext = viewModel;
            viewModel.OnSuccess += (s, e) =>
            {

            };
        }

        private void NavigateToAddAccount(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddAccountPage());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;

            var swipeDirection = label.Bounds.X > offsetStart ? SwipeDirection.Left : SwipeDirection.Right;
            Swiped(sender, new SwipedEventArgs(null, swipeDirection));
        }

        private void DoubleTaped(object sender, System.EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            GoToChargesList(stackLayout);
        }

        private async void Swiped(object sender, SwipedEventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;

            if (IsAlreadySwiped(label, e.Direction)) return;

            var buttonsActions = stackLayout.Children.Where(x => x.GetType().Equals(typeof(Button)));

            var offsetIn = e.Direction.Equals(SwipeDirection.Right) ? offsetStart : -offsetStart;
            var offsetOut = e.Direction.Equals(SwipeDirection.Right) ? offsetEnd : -offsetEnd;

            var rectIn = label.Bounds.Offset(offsetIn, 0);
            var rectOut = label.Bounds.Offset(offsetOut, 0);

            var animationIn = await label.LayoutTo(rectIn, easing: Easing.SpringIn);

            if (e.Direction.Equals(SwipeDirection.Left))
            {
                DisplayButton(buttonsActions, e.Direction.Equals(SwipeDirection.Right));
                rectOut.X = 0;
            }

            var animationOut = await label.LayoutTo(rectOut, easing: Easing.SpringOut);

            DisplayButton(buttonsActions, e.Direction.Equals(SwipeDirection.Right));
        }

        private void DisplayButton(IEnumerable<View> buttons, bool visible)
        {
            foreach (var button in buttons)
            {
                button.IsVisible = visible;
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent as StackLayout;
            var account = GetCurrentAccount(stacklayout);

            var editPage = new EditAccountPage(account);

            MessagingCenter.Subscribe<EditAccountPage>(editPage, Constants.Messages.UpdateOnAppearing, page =>
           {
               viewModel.Refresh();
               MessagingCenter.Unsubscribe<EditAccountPage>(editPage, Constants.Messages.UpdateOnAppearing);
           });

            Navigation.PushAsync(editPage);
        }

        private void ViewChargesList(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent as StackLayout;
            GoToChargesList(stacklayout);
        }

        private async void Delete(object sender, EventArgs e)
        {
            var result = await modal.ShowOkCancel(localizationManager.Translate("DeleteChargeModal"));
            if (result)
            {
                var deleteButton = sender as Button;
                var stacklayout = deleteButton.Parent as StackLayout;
                var account = GetCurrentAccount(stacklayout);
                await viewModel.Delete(account);
                viewModel.Refresh();
            }
        }

        private void GoToChargesList(StackLayout stackLayout)
        {
            var account = GetCurrentAccount(stackLayout);
            Navigation.PushAsync(new ChargesList(account));
        }

        private bool IsAlreadySwiped(Label label, SwipeDirection direction)
                        => label.Bounds.X > offsetStart && direction.Equals(SwipeDirection.Right) ||
                            label.Bounds.X < offsetStart && direction.Equals(SwipeDirection.Left);

        private Account GetCurrentAccount(StackLayout stackLayout)
        {
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;
            var accounts = ((AccountsViewModel)accountsList.BindingContext).Accounts;
            return accounts.First(x => x.Name.Equals(label.Text));
        }
    }
}