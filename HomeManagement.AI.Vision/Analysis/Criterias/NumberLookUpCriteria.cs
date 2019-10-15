using System.Linq;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class NumberLookUpCriteria : IMatchingCriteria, IMatch
    {
        public bool IsMatch(char c1) => char.IsNumber(c1);

        public bool IsMatch(string value)
        {
            var match = value.Any(x => IsMatch(x));
            return match;
        }
    }
}
