﻿using HomeManagement.App.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        DashboardViewModel dashboardViewModel = new DashboardViewModel();

        public DashboardPage()
        {
            InitializeComponent();

            BindingContext = dashboardViewModel;

            dashboardViewModel.OnInitializationError += DashboardViewModel_OnError;

            dashboardViewModel.OnInitializationFinished += OnInitialized;
        }

        private void DashboardViewModel_OnError(object sender, System.EventArgs e)
        {
            DisplayAlert("Error", "Something went worng....", string.Empty);
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AccountPages.AccountPage());
        }

        private void OnInitialized(object sender, System.EventArgs e)
        {
            if (dashboardViewModel.Notifications.Any())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ToolbarItems.Clear();
                    var item = new ToolbarItem
                    {
                        IconImageSource = ImageSource.FromFile("notifications_24dp.png")
                    };

                    item.Clicked += (s, ev) =>
                    {
                        Navigation.PushAsync(new NotificationsPage());
                    };

                    ToolbarItems.Add(item);
                });                
            }
        }

        private void GoToSettings(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Settings());
        }
    }
}