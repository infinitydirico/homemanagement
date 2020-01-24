using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfflinePage : ContentPage
    {
        public OfflinePage()
        {
            InitializeComponent();

            Connectivity.ConnectivityChanged += (sender, args) =>
            {
                if (args.NetworkAccess.Equals(NetworkAccess.Internet))
                {
                    Navigation.RemovePage(this);
                }
            };
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}