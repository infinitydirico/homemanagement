namespace HomeManagement.AI.Vision.Analysis.Criterias
{
    public class NumberLookUpCriteria : ILookUpCriteria
    {
        public bool IsMatch(char c1) => char.IsNumber(c1) || char.IsPunctuation(c1);
    }
}
