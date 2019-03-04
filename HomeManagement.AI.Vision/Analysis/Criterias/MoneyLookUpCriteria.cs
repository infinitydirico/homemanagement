using System.Globalization;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class MoneyLookUpCriteria : ILookUpCriteria
    {
        public bool IsMatch(char c1) => 
            char.IsNumber(c1) ||
            char.IsPunctuation(c1) || 
            char.GetUnicodeCategory(c1).Equals(UnicodeCategory.CurrencySymbol);
    }
}
