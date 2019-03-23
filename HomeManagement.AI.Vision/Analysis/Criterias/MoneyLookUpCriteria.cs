using System.Globalization;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class MoneyLookUpCriteria : ILookUpCriteria
    {
        public bool SearchNearRows { get; set; }

        public bool TryDeepParsing => false;

        public bool IsMatch(char c1) =>
            char.GetUnicodeCategory(c1).Equals(UnicodeCategory.CurrencySymbol);

        public bool IsParseable(string value) => false;
    }
}
