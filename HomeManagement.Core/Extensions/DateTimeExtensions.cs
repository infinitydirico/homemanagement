using System;

namespace HomeManagement.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetPreviousMonth(this DateTime date)
        {
            return DateTime.Now.AddMonths(-1);
        }

        public static string GetCurrentMonth(this DateTime date, int month)
        {
            var monthName = new DateTime(date.Year, month, date.Day).ToString("MMMM");
            return monthName;
        }

        public static bool IsSameDay(this DateTime dateTime, DateTime date)
            => dateTime.Year.Equals(date.Year) &&
                dateTime.Month.Equals(date.Month) &&
                dateTime.Day.Equals(date.Day);

        public static bool IsSameMonth(this DateTime dateTime, DateTime date)
            => dateTime.Year.Equals(date.Year) && dateTime.Month.Equals(date.Month);

        public static bool IsNextMonth(this DateTime dateTime, DateTime date)
        {
            var previousMonth = dateTime.AddMonths(-1);
            return date.IsSameMonth(previousMonth);
        }

        public static bool IsPreviousMonth(this DateTime dateTime, DateTime date)
        {
            var nextMonth = dateTime.AddMonths(1);
            return date.IsSameMonth(nextMonth);
        }
    }
}
