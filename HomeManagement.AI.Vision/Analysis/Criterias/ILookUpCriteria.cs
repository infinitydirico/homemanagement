using System.Collections.Generic;

namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public interface ILookUpCriteria
    {
        bool SearchNearRows { get; }

        bool TryDeepParsing { get; }

        bool IsMatch(char c1);

        bool IsParseable(string value);
    }
}
