using System;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class DateLookUpCriteria : ILookUpCriteria
    {
        public bool SearchNearRows { get; set; }

        public bool TryDeepParsing => true;

        public bool IsMatch(char c1) => false;

        public bool IsParseable(string value) => DateTime.TryParse(value, out DateTime date) || TimeSpan.TryParse(value, out TimeSpan timeSpan);
    }
}
