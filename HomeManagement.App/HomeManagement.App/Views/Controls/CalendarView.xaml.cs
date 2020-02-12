using HomeManagement.App.Extensions;
using HomeManagement.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentView
    {
        private uint animationTimeout = 150;
        private List<Tuple<DateTime, Frame>> gridValues = new List<Tuple<DateTime, Frame>>();
        private Frame selectedDate;

        public CalendarView()
        {
            InitializeComponent();

            Initialize();
        }

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create("Date", typeof(DateTime), typeof(CalendarView), DateTime.Now);

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly BindableProperty EventsProperty =
            BindableProperty.Create("Events",
                                    typeof(List<EventDate>),
                                    typeof(CalendarView),
                                    new List<EventDate>(),
                                    propertyChanged: OnEventsChanged);

        public List<EventDate> Events
        {
            get { return (List<EventDate>)GetValue(EventsProperty); }
            set { SetValue(EventsProperty, value); }
        }

        public event EventHandler OnDateChanged;

        static async void OnEventsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CalendarView;

            if (view.Events == null) return;

            if(view.Events.Any(x => x.Date.IsSameMonth(view.Date)))
            {
                var values = view.gridValues.Where(x => view.Events.Any(z => z.Date.IsSameDay(x.Item1))).ToList();

                foreach (var value in values)
                {
                    await value.Item2.BorderColorTo(Color.Red, 100);
                }
            }           
        }

        private async void Initialize()
        {
            calendarGrid.Opacity = 0;

            FillGrid();

            await calendarGrid.FadeTo(1, 1500);
        }

        private void FillGrid()
        {
            calendarGrid.RowDefinitions.Clear();
            calendarGrid.Children.Clear();

            InitializeGridHeaders();

            var startingRow = 2;
            var matrix = Matrix();
            for (int i = 0; i < matrix.Count; i++)
            {
                var daysOfWeek = matrix[i];

                calendarGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(30, GridUnitType.Absolute),
                });

                foreach (var date in daysOfWeek)
                {
                    var label = new Label
                    {
                        Text = date.Day.ToString(),
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };

                    var frame = new Frame
                    {
                        Content = label,
                        Padding = new Thickness(0),
                        Margin = new Thickness(0),
                        CornerRadius = 360
                    };

                    if (date.IsSameDay(DateTime.Now))
                    {
                        frame.BackgroundColor = Color.FromRgb(50, 125, 225);
                    }

                    if(Events.Any(x => x.Date.IsSameDay(date)))
                    {
                        frame.BorderColor = Color.Red;
                    }

                    var gesture = new TapGestureRecognizer();
                    gesture.Tapped += OnFrameTapped;
                    frame.GestureRecognizers.Add(gesture);

                    calendarGrid.Children.Add(frame, (int)date.DayOfWeek, i + startingRow);
                    gridValues.Add(new Tuple<DateTime, Frame>(date, frame));
                }
            }
        }

        private async void OnFrameTapped(object sender, EventArgs e)
        {
            eventsView.Children.Clear();

            var value = gridValues.FirstOrDefault(x => x.Item2.Equals(sender));

            if(value != null)
            {
                var events = Events.Where(ev => ev.Date.IsSameDay(value.Item1)).ToList();

                var title = new Label
                {
                    Text = value.Item1.ToString("dd MMMM yyyy"),
                    Margin = new Thickness(0, 20),
                    FontSize = 20
                };

                eventsView.Children.Add(title);

                if(selectedDate != null)
                {
                    await selectedDate.BackgroundColorTo(Color.Transparent, 100);
                }

                selectedDate = sender as Frame;
                await selectedDate.BackgroundColorTo(Color.Purple, 100);

                foreach (var ev in events)
                {
                    var label = new Label
                    {
                        Text = ev.Title,
                        Opacity = 0
                    };

                    eventsView.Children.Add(label);
                    await label.FadeTo(1);
                }
            }
        }

        private void InitializeGridHeaders()
        {
            InitializeFirstHeader();

            InitializeSecondHeader();
        }

        private void InitializeFirstHeader()
        {
            calendarGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(30, GridUnitType.Absolute)
            });

            var previousButton = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 35,
                HeightRequest = 35,
                CornerRadius = 360,
                Image = "arrow_left_18dp"
            };
            previousButton.Clicked += PreviousButton_Clicked;

            calendarGrid.Children.Add(previousButton, 0, 0);

            calendarGrid.Children.Add(new Label
            {
                Text = Date.ToString("MMMM yyyy"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            }, 1, 5, 0, 1);

            var nextButton = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 35,
                HeightRequest = 35,
                CornerRadius = 360,
                Image = "arrow_right_18dp"
            };

            nextButton.Clicked += NextButton_Clicked;

            calendarGrid.Children.Add(nextButton, 6, 0);
        }

        private void InitializeSecondHeader()
        {
            calendarGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(30, GridUnitType.Absolute)
            });

            calendarGrid.Children.Add(new Label
            {
                Text = DayOfWeek.Sunday.ToString().Substring(0, 3),
                HorizontalOptions = LayoutOptions.Center
            }, 0, 1);

            calendarGrid.Children.Add(new Label
            {
                Text = DayOfWeek.Monday.ToString().Substring(0, 3),
                HorizontalOptions = LayoutOptions.Center
            }, 1, 1);

            calendarGrid.Children.Add(new Label
            {
                Text = DayOfWeek.Tuesday.ToString().Substring(0, 3),
                HorizontalOptions = LayoutOptions.Center
            }, 2, 1);

            calendarGrid.Children.Add(new Label
            {
                Text = DayOfWeek.Wednesday.ToString().Substring(0, 3),
                HorizontalOptions = LayoutOptions.Center
            }, 3, 1);

            calendarGrid.Children.Add(new Label
            {
                Text = DayOfWeek.Thursday.ToString().Substring(0, 3),
                HorizontalOptions = LayoutOptions.Center
            }, 4, 1);

            calendarGrid.Children.Add(new Label
            {
                Text = DayOfWeek.Friday.ToString().Substring(0, 3),
                HorizontalOptions = LayoutOptions.Center
            }, 5, 1);

            calendarGrid.Children.Add(new Label
            {
                Text = DayOfWeek.Saturday.ToString().Substring(0, 3),
                HorizontalOptions = LayoutOptions.Center
            }, 6, 1);
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            eventsView.Children.Clear();

            Date = Date.AddMonths(1);

            var offset = Width + 50;
            var rect = calendarGrid.Bounds.Offset(-offset, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, animationTimeout, Easing.CubicOut);

            FillGrid();

            rect = calendarGrid.Bounds.Offset(offset * 2, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, 50, Easing.CubicIn);

            rect = calendarGrid.Bounds.Offset(-offset, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, animationTimeout, Easing.CubicOut);

            OnDateChanged.Invoke(this, EventArgs.Empty);
        }

        private async void PreviousButton_Clicked(object sender, EventArgs e)
        {
            eventsView.Children.Clear();

            Date = Date.AddMonths(-1);

            var offset = Width + 50;
            var rect = calendarGrid.Bounds.Offset(offset, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, animationTimeout, Easing.CubicOut);

            FillGrid();

            rect = calendarGrid.Bounds.Offset(-offset * 2, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, 50, Easing.CubicIn);

            rect = calendarGrid.Bounds.Offset(offset, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, animationTimeout, Easing.CubicOut);

            OnDateChanged.Invoke(this, EventArgs.Empty);
        }

        private List<List<DateTime>> Matrix()
        {
            var firstWeek = GetFirstWeek();
            List<List<DateTime>> matrix = new List<List<DateTime>>
            {
                firstWeek
            };

            var nextDayWeek = firstWeek.Last().AddDays(1);
            while (true)
            {
                var week = PadRight(nextDayWeek);
                matrix.Add(week);
                nextDayWeek = nextDayWeek.AddDays(7);

                if (nextDayWeek.IsNextMonth(Date) || nextDayWeek.IsPreviousMonth(Date)) break;
            }

            return matrix;
        }

        private List<DateTime> GetFirstWeek()
        {
            var date = Date.AddDays(-(Date.Day - 1));
            List<DateTime> days = new List<DateTime>();

            days.AddRange(PadRight(date));
            return days;
        }

        private List<DateTime> PadRight(DateTime date)
        {
            List<DateTime> days = new List<DateTime>
            {
                date
            };

            while (!date.DayOfWeek.Equals(DayOfWeek.Saturday))
            {
                date = date.AddDays(1);

                if (date.IsNextMonth(Date) || date.IsPreviousMonth(Date)) break;

                days.Add(date);
            }

            return days;
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            if (e.Direction.Equals(SwipeDirection.Right))
            {
                PreviousButton_Clicked(null, EventArgs.Empty);
            }

            if (e.Direction.Equals(SwipeDirection.Left))
            {
                NextButton_Clicked(null, EventArgs.Empty);
            }
        }
    }

    public class EventDate
    {
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public override string ToString() => $"{Title} {Date.ToShortDateString()}";
    }
}