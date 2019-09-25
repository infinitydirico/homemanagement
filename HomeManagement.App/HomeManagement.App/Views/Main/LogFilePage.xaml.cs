using HomeManagement.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogFilePage : ContentPage
	{
		public LogFilePage ()
		{
			InitializeComponent ();

            logtext.Text = Logger.ReadLog();

        }
	}
}