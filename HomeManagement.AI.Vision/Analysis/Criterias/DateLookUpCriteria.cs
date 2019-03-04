using System.Collections.Generic;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class DateLookUpCriteria : ILookUpCriteria
    {
        public bool IsMatch(char c1) => true;//char.GetUnicodeCategory(c1).Equals(char.GetUnicodeCategory(c2));
    }
}
