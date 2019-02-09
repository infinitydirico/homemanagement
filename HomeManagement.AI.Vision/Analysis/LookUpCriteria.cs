using System.Collections.Generic;

namespace HomeManagement.AI.Vision.Analysis
{
    public abstract class LookUpCriteria
    {
        public abstract bool Match(char c1, char c2);

        public virtual IEnumerable<string> GetPossibleCharacters()
        {
            yield return string.Empty;
        }
    }
}
