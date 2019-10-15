using System.Globalization;
using System.Linq;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class MoneyLookUpCriteria : IMatchingCriteria, IMatch
    {
        public bool IsMatch(char c1) =>
            char.GetUnicodeCategory(c1).Equals(UnicodeCategory.CurrencySymbol);

        public bool IsMatch(string value)
        {
            var match = value.Any(x => IsMatch(x));
            return match;
        }
    }
}
