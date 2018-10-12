using System;

namespace HomeManagement.Core.Extensions
{
    public static class ArithmeticExtensions
    {
        public static int CalculatePercentage(this int currentValue, int previousValue)
        {
            var diff = currentValue - previousValue;

            var percentageValue = previousValue.Equals(default(int)) ? 1 : decimal.Parse(diff.ToString()) / decimal.Parse(previousValue.ToString());

            var percentage = percentageValue * 100;

            return Convert.ToInt32(percentage);
        }

        public static int CalculatePercentage(this decimal currentValue, decimal previousValue)
        {
            var diff = currentValue - previousValue;

            var percentageValue = previousValue.Equals(decimal.Zero) ? 1 : diff / previousValue;

            var percentage = percentageValue * 100;

            return Convert.ToInt32(percentage);
        }

        public static int Fibonacci(this int n) => n > 1 ? Fibonacci(n - 1) + Fibonacci(n - 2) : 1;
    }
}
