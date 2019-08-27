using HomeManagement.App.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Charges
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarPage : ContentPage
	{
        Account account;

        public CalendarPage (Account account)
		{
            this.account = account;

            Title = account.Name;

            InitializeComponent ();

            calendar.Events = new List<Controls.EventDate>
            {
                new Controls.EventDate
                {
                    Title = "Test 1",
                    Date = DateTime.Now.AddDays(-2)
                },
                new Controls.EventDate
                {
                    Title = "Test 2",
                    Date = DateTime.Now.AddDays(2)
                },
                new Controls.EventDate
                {
                    Title = "Test 3",
                    Date = DateTime.Now.AddDays(2)
                },
                new Controls.EventDate
                {
                    Title = "Test 4",
                    Date = DateTime.Now.AddDays(2)
                },
                new Controls.EventDate
                {
                    Title = "Test 5",
                    Date = DateTime.Now.AddDays(2)
                },
                new Controls.EventDate
                {
                    Title = "Test 6",
                    Date = DateTime.Now.AddDays(2)
                },
            };
        }
	}
}