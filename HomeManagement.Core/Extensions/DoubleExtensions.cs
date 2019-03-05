namespace HomeManagement.Core.Extensions
{
    public static class DoubleExtensions
    {
        public static int ParseNoDecimals(this double value) => int.Parse(value.ToString("F0"));
    }
}
