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

        public static TEnum To<TEnum>(this Enum value)
        {
            return ToEnum<TEnum>(value.ToString());
        }

        public static ComparingOperators IntToOperator(this int value) => ToEnum<ComparingOperators>(value.ToString());
    }
}
