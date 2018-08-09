using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.App.Services.Components
{
    public class ChartsFactory
    {
        static List<SKColor> colours = new List<SKColor>
        {
            SKColor.Parse("#87D68D"),
            SKColor.Parse("#FFF6E5"),
            SKColor.Parse("#F4D0A6"),
            SKColor.Parse("#82A5FF"),
            SKColor.Parse("#EAFF63"),
            SKColor.Parse("#B09BCC"),
            SKColor.Parse("#8EE5A9"),
            SKColor.Parse("#F44E83"),
            SKColor.Parse("#E89366"),
            SKColor.Parse("#B6BA9C"),
            SKColor.Parse("#82A0ED"),
        };

        public static Chart CreateBarChart(Dictionary<string, string> values)
        {
            return new BarChart()
            {
                Entries = CreateEntries(values),
                BackgroundColor = new SKColor(48, 48, 48),
            };
        }

        public static Chart CreateLineChart(Dictionary<string, string> values)
        {
            return new LineChart()
            {
                Entries = CreateEntries(values),
                BackgroundColor = new SKColor(48, 48, 48),
            };
        }

        public static Chart CreateDonutChart(Dictionary<string, string> values)
        {
            return new DonutChart()
            {
                Entries = CreateEntries(values),
                BackgroundColor = new SKColor(48, 48, 48),
            };
        }

        public static Chart CreatePointChart(Dictionary<string, string> values)
        {
            return new PointChart()
            {
                Entries = CreateEntries(values),
                BackgroundColor = new SKColor(48, 48, 48),
            };
        }

        public static Chart CreateRadarChart(Dictionary<string, string> values)
        {
            return new RadarChart()
            {
                Entries = CreateEntries(values),
                BackgroundColor = new SKColor(48, 48, 48),
            };
        }

        private static IEnumerable<Entry> CreateEntries(Dictionary<string, string> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                var colour = colours[i];
                var item = values.ElementAt(i);

                yield return new Entry(float.Parse(item.Value))
                {
                    ValueLabel = item.Value,
                    Label = item.Key,
                    Color = colour,
                    TextColor = SKColors.White
                };
            }
        }
    }
}
