using System;

namespace HomeManagement.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetPreviousMonth(this DateTime date)
        {
            return DateTime.Now.AddMonths(-1);
        }
    }
}
