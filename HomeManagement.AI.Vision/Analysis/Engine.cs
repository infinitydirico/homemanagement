using HomeManagement.AI.Vision.Entities;
using HomeManagement.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace HomeManagement.AI.Vision.Analysis
{
    public class Engine
    {
        public int Matching { get; set; } = 80;

        public IEnumerable<string> GetAllMatches(VisionRecognitionResult visionRecognitionResult, WordLookUp wordLookUp)
        {
            foreach (var line in visionRecognitionResult.Lines)
            {
                var match = wordLookUp.GetMatch(line.Text);

                var onSameColumns = visionRecognitionResult
                    .Lines
                    .Where(x => DoColumnMatchingValidation(x, line, wordLookUp))
                    .ToList();

                var onSameRows = visionRecognitionResult
                    .Lines
                    .Where(x => DoRowMatchingValidation(x, line, wordLookUp))
                    .ToList();

                if (match > Matching)
                {
                    Debug.WriteLine($"word is {line.Text} and has {match}% of matching");
                    yield return line.Text;
                }
            }
        }

        private bool DoColumnMatchingValidation(Line l1, Line l2, WordLookUp wordLookUp)
        {
            return l1.IsOnSameColumn(l2) && !l1.Equals(l2) &&
                wordLookUp.Criteria.GetPossibleCharacters().Any(z => l1.Text.Contains(z) && !string.IsNullOrEmpty(z));
        }

        private bool DoRowMatchingValidation(Line l1, Line l2, WordLookUp wordLookUp)
        {
            return l1.IsOnSameRow(l2) && !l1.Equals(l2) &&
                wordLookUp.Criteria.GetPossibleCharacters().Any(z => l1.Text.Contains(z) && !string.IsNullOrEmpty(z));
        }
    }
}
