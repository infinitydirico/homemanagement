using HomeManagement.App.ViewModels.Statistics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Statistics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensiveCategoriesChart : Frame
    {
        public ExpensiveCategoriesChart()
        {
            InitializeComponent();
            Opacity = 0;

            ((ExpensiveCategoriesViewModel)BindingContext).OnInitializationFinished += (s, e) =>
            {
                this.FadeTo(0.5, 500, Easing.SpringIn);
                this.FadeTo(1, 500, easing: Easing.SpringOut);
            };
        }
    }
}