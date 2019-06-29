using Plugin.Connectivity;

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

            CrossConnectivity.Current.ConnectivityTypeChanged += (sender, args) =>
            {
                if (args.IsConnected)
                {
                    Navigation.RemovePage(this);
                }
            };
        }
    }
}