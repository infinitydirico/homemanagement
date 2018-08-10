using System;

namespace HomeManagement.Core.Extensions
{
    public static class EnumExtensions
    {
        public static TEnum ToEnum<TEnum>(this string value)
        {
            var enumValue = Enum.Parse(typeof(TEnum), value);
            var castedEnum = (TEnum)enumValue;
            return castedEnum;
        }
    }
}
