using System.Linq;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class TextLookUpCriteria : IMatchingCriteria, IMatch
    {
        public bool IsMatch(char c1) =>
            char.IsLetter(c1) &&
            !char.IsNumber(c1) &&
            !char.IsPunctuation(c1) &&
            !char.IsSymbol(c1);

        public bool IsMatch(string value)
        {
            var match = value.Any(x => IsMatch(x));
            return match;
        }
    }
}
