namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class NumberLookUpCriteria : ILookUpCriteria
    {
        public bool SearchNearRows { get; set; }

        public bool TryDeepParsing => false;

        public bool IsMatch(char c1) => char.IsNumber(c1) || char.IsPunctuation(c1);

        public bool IsParseable(string value) => false;
    }
}
