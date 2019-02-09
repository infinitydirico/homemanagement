using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> OneElement<T>(this T value)
        {
            var enumerable = Enumerable.Empty<T>();
            enumerable.Append(value);
            return enumerable;
        }
    }
}
