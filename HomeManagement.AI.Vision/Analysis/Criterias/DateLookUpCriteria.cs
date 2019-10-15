using System;
using System.Globalization;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class DateLookUpCriteria : IMatch
    {
        public bool IsMatch(string value)
        {
            var divider = value.Contains("/") ? "/" : "-";

            var templateDate = $"dd{divider}MM{divider}yyyy";

            templateDate = value.Length.Equals(templateDate.Length) ?
                templateDate : 
                templateDate.Substring(0, templateDate.Length - 2);

            var canParse = DateTime.TryParseExact(value, templateDate,
                CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var date);

            return canParse;
        }
    }
}
