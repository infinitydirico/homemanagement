namespace HomeManagement.AI.Vision.Analysis
{
    public class NumberLookUpCriteria : LookUpCriteria
    {
        public override bool Match(char c1, char c2) => char.IsNumber(c1) && char.IsNumber(c2) || char.IsPunctuation(c1) && char.IsPunctuation(c2);
    }
}
