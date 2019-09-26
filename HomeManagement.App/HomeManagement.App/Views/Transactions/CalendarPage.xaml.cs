using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarPage : ContentPage
	{
        Account account;
        CalendarViewModel viewModel;

        public CalendarPage (Account account)
		{
            this.account = account;

            viewModel = new CalendarViewModel(this.account);
            BindingContext = viewModel;

            Title = account.Name;

            InitializeComponent ();

            calendar.OnDateChanged += Calendar_OnDateChanged;
        }

        private async void Calendar_OnDateChanged(object sender, EventArgs e)
        {
            await viewModel.ChangeDate(calendar.Date);
        }
    }
}