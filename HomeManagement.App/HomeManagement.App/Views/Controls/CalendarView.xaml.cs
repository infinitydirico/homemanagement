using HomeManagement.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentView
    {
        private uint animationTimeout = 250;

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

        static void OnEventsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CalendarView;

            if(view.Events.Any(x => x.Date.IsSameMonth(view.Date)))
            {
                var days = view.calendarGrid.Children
                    .Where(x => x is Frame &&
                                view.Events.Any(z => z.Date.Day.Equals(((x as Frame).Content as Label).Text.ToInt())))
                    .ToList();

                foreach (var day in days)
                {
                    (day as Frame).BorderColor = Color.Red;
                }
                //foreach (var child in view.calendarGrid.Children)
                //{
                //    if(child is Frame)
                //    {
                //        var frame = child as Frame;
                //        var label = frame.Content as Label;
                //        var day = label.Text.ToInt();

                //        var ev = view.Events.First(x => x.Date.Day.Equals(day));
                //    }                    
                //}
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

                    calendarGrid.Children.Add(frame, (int)date.DayOfWeek, i + startingRow);
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
                Text = "<",
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 35,
                HeightRequest = 35,
                FontSize = 10,
                CornerRadius = 360
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
                Text = ">",
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 35,
                HeightRequest = 35,
                FontSize = 10,
                CornerRadius = 360
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
            Date = Date.AddMonths(1);

            var offset = Width + 50;
            var rect = calendarGrid.Bounds.Offset(-offset, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, animationTimeout, Easing.SpringOut);

            FillGrid();

            rect = calendarGrid.Bounds.Offset(offset * 2, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, 50, Easing.SpringIn);

            rect = calendarGrid.Bounds.Offset(-offset, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, animationTimeout, Easing.SpringOut);
        }

        private async void PreviousButton_Clicked(object sender, EventArgs e)
        {
            Date = Date.AddMonths(-1);

            var offset = Width + 50;
            var rect = calendarGrid.Bounds.Offset(offset, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, animationTimeout, Easing.SpringOut);

            FillGrid();

            rect = calendarGrid.Bounds.Offset(-offset * 2, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, 50, Easing.SpringIn);

            rect = calendarGrid.Bounds.Offset(offset, calendarGrid.Bounds.Y);
            await calendarGrid.LayoutTo(rect, animationTimeout, Easing.SpringOut);
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

        public Color Color { get; set; }
    }
}