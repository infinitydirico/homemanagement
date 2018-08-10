using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace HomeManagement.Core.Extensions
{
    public static class ExpressionExtensions
    {
        public static Func<T, bool> Where<T>(string property, string value, ComparingOperators valueOperator)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            MemberExpression member = Expression.Property(param, property);

            Type propertyType = member.Type;
            TypeConverter converter = TypeDescriptor.GetConverter(propertyType);
            var converted = converter.ConvertFrom(value.ToString());
            ConstantExpression constant = Expression.Constant(converted);

            Expression comparingExpression = default(BinaryExpression);

            switch (valueOperator)
            {
                case ComparingOperators.EqualOperator:
                    comparingExpression = Expression.Equal(member, Expression.Convert(constant, member.Type));
                    break;
                case ComparingOperators.GreaterOperator:
                    comparingExpression = Expression.GreaterThan(member, Expression.Convert(constant, member.Type));
                    break;
                case ComparingOperators.GreaterThanOrEqualOperator:
                    comparingExpression = Expression.GreaterThanOrEqual(member, Expression.Convert(constant, member.Type));
                    break;
                case ComparingOperators.LessOperator:
                    comparingExpression = Expression.LessThan(member, Expression.Convert(constant, member.Type));
                    break;
                case ComparingOperators.LessThanOrEqualOperator:
                    comparingExpression = Expression.LessThanOrEqual(member, Expression.Convert(constant, member.Type));
                    break;
                case ComparingOperators.Like:
                    var constantValue = Expression.Constant(value);
                    var stringType = typeof(string);
                    var method = stringType.GetMethod("Contains");
                    comparingExpression = Expression.Call(member, method, constant); //, member, Expression.Convert(constant, member.Type));
                    //comparingExpression = Expression.Call(method, member, Expression.Convert(constant, member.Type));
                    break;
                default:
                    comparingExpression = Expression.Equal(member, Expression.Convert(constant, member.Type));
                    break;
            }

            //comparingExpression = Expression.Equal(member, Expression.Convert(constant, member.Type));
            var lambda = Expression.Lambda<Func<T, bool>>(comparingExpression, param);

            var compiledLambda = lambda.Compile();

            return compiledLambda;
        }

        public static Func<T, bool> Where<T>(this Func<T, bool> predicate, string property, string value, ComparingOperators valueOperator)
        {
            var clause = Where<T>(property, value, valueOperator);

            Func<T, bool> stat = o => clause(o) && predicate(o);

            return stat;
        }
    }

    public enum ComparingOperators
    {
        EqualOperator,
        GreaterOperator,
        GreaterThanOrEqualOperator,
        LessOperator,
        LessThanOrEqualOperator,
        Like
    }
}
