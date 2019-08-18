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
    }
}
