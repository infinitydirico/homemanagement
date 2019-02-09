using System.Collections.Generic;

namespace HomeManagement.AI.Vision.Analysis
{
    public class DateLookUpCriteria : LookUpCriteria
    {
        public override bool Match(char c1, char c2) => char.GetUnicodeCategory(c1).Equals(char.GetUnicodeCategory(c2));

        public override IEnumerable<string> GetPossibleCharacters()
        {
            yield return "/";
        }
    }
}
