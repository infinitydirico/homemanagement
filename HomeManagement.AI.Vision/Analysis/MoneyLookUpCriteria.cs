using System.Collections.Generic;
using System.Globalization;

namespace HomeManagement.AI.Vision.Analysis
{
    public class MoneyLookUpCriteria : LookUpCriteria
    {
        public override bool Match(char c1, char c2) => 
            char.IsNumber(c1) && char.IsNumber(c2) ||
            char.IsPunctuation(c1) || 
            char.GetUnicodeCategory(c1).Equals(UnicodeCategory.CurrencySymbol) || char.GetUnicodeCategory(c2).Equals(UnicodeCategory.CurrencySymbol);

        public override IEnumerable<string> GetPossibleCharacters()
        {
            yield return "$";
        }
    }
}
