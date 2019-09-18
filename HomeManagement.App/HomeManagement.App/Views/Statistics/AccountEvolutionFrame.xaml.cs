using Nightingale;
using Nightingale.Charts;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Statistics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountEvolutionFrame : Frame
    {
        List<SeriesValue> series = new List<SeriesValue>();

        public AccountEvolutionFrame()
        {
            InitializeComponent();
        }

        public List<SeriesValue> Series
        {
            get => series;
            set
            {
                series = value;
                DrawInnerContent();
            }
        }

        public static readonly BindableProperty SeriesProperty = BindableProperty.Create(nameof(Series),
            typeof(List<SeriesValue>), typeof(AccountEvolutionFrame));

        public string AccountName { get; set; }

        public static readonly BindableProperty AccountNameProperty = BindableProperty.Create(nameof(AccountName),
            typeof(string), typeof(AccountEvolutionFrame));

        private void DrawInnerContent()
        {
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
                Text = AccountName
            };

            chart.Series = Series;

            layout.Children.Add(label);
            layout.Children.Add(chart);

            Content = layout;
        }
    }
}