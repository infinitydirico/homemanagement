using HomeManagement.AI.Vision.Analysis.Criterias;
using HomeManagement.AI.Vision.Entities;
using HomeManagement.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.AI.Vision.Analysis
{
    public class Engine
    {
        public IEnumerable<ILookUpCriteria> Criterias { get; set; }

        public IEnumerable<string> GetAllMatches(VisionRecognitionResult visionRecognitionResult)
        {
            foreach (var line in visionRecognitionResult.Lines)
            {
                int criteriasMatching = 0;
                foreach (var criteria in Criterias)
                {
                    if((line.Text.RemoveEmptySpaces().All(x => !char.IsWhiteSpace(x) && criteria.IsMatch(x)) && !criteria.TryDeepParsing) ||
                        (criteria.SearchNearRows && HasNearByMatchingCriteria(visionRecognitionResult.Lines, line, criteria) && !criteria.TryDeepParsing) ||
                        (criteria.TryDeepParsing && criteria.IsParseable(line.Text)))
                    {
                        criteriasMatching++;
                        continue;
                    }
                }

                if (criteriasMatching.Equals(Criterias.Count()))
                {
                    yield return line.Text;
                }
            }
        }

        private bool HasNearByMatchingCriteria(List<Line> lines, Line line, ILookUpCriteria criteria)
        {
            return (from l in lines
                    where !line.Equals(l) &&
                             (l.IsOnSameColumn(line) || l.IsOnSameRow(line))
                    select l).All(z => z.Text.RemoveEmptySpaces().Any(x => criteria.IsMatch(x)));
        }
    }
}
