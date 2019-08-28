using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using Nightingale.Charts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsPage : ContentPage
    {
        StatisticsViewModel viewModel = new StatisticsViewModel();
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public StatisticsPage()
        {
            InitializeComponent();

            BindingContext = viewModel;

            viewModel.OnBalancesChanged += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var evolution in viewModel.AccountsEvolutions)
                    {
                        var frame = new Frame
                        {
                            Margin = new Thickness(5)
                        };

                        var layout = new StackLayout();
                        var chart = new LineChart
                        {
                            TextSize = 25,
                            BackgroundColor = Color.FromHex("#303030"),
                            HeightRequest = 150,
                            RenderArea = true
                        };

                        var label = new Label
                        {
                            Text = evolution.AccountName
                        };

                        chart.Series = evolution.Series;

                        layout.Children.Add(label);
                        layout.Children.Add(chart);

                        frame.Content = layout;

                        mainContainer.Children.Add(frame);
                    }
                });
            };

            Title = localizationManager.Translate("Statistics");

            if (Device.Idiom.Equals(TargetIdiom.Desktop))
            {
                barChart.TextSize = 12;
            }
        }
    }
}