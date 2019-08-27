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

            InitializeComponent ();

            calendar.Events = new List<Controls.EventDate>
            {
                new Controls.EventDate
                {
                    Title = "Test1",
                    Date = DateTime.Now.AddDays(-2),
                    Color = Color.Red
                },
                new Controls.EventDate
                {
                    Title = "Test2",
                    Date = DateTime.Now.AddDays(2),
                    Color = Color.Purple
                }
            };
        }
	}
}