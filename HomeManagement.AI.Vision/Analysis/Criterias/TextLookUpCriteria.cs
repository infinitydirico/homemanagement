namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class TextLookUpCriteria : ILookUpCriteria
    {
        public bool SearchNearRows { get; set; }

        public bool TryDeepParsing => false;

        public bool IsMatch(char c1) =>
            char.IsLetter(c1) &&
            !char.IsNumber(c1) &&
            !char.IsPunctuation(c1) &&
            !char.IsSymbol(c1);

        public bool IsParseable(string value) => false;
    }
}
