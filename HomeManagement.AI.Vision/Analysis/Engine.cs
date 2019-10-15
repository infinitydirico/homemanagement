using HomeManagement.AI.Vision.Analysis.Criterias;
using HomeManagement.AI.Vision.Entities;
using HomeManagement.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.AI.Vision.Analysis
{
    public class Engine
    {
        public IEnumerable<IMatch> Criterias { get; set; }

        public IEnumerable<string> GetAllMatches(VisionRecognitionResult visionRecognitionResult)
        {
            var possibleMatches = visionRecognitionResult.Lines
                .Where(IsMatch)
                .Select(x => x.Text)
                .ToList();

            return possibleMatches;
        }

        public bool IsMatch(Box line)
        {
            foreach (var criteria in Criterias)
            {
                var match = criteria.IsMatch(line.Text);

                if (match) return true;
            }
            return false;
        }
    }
}
