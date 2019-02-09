namespace HomeManagement.AI.Vision.Analysis
{
    public class TextLookUpCriteria : LookUpCriteria
    {
        public override bool Match(char c1, char c2) =>
            (char.IsLetter(c1) && char.IsLetter(c2)) &&
            !(char.IsNumber(c1) && char.IsNumber(c2)) &&
            !(char.IsPunctuation(c1) && char.IsPunctuation(c2)) &&
            !(char.IsSymbol(c1) && char.IsSymbol(c2));
    }
}
