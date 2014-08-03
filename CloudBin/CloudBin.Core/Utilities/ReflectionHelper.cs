using System;
using System.Linq.Expressions;

namespace CloudBin.Core.Utilities
{
    public static class ReflectionHelper
    {
        public static string GetParamName<T>(Expression<Func<T>> paramExpression)
        {
            var expression = (MemberExpression) paramExpression.Body;
            return expression.Member.Name;
        }
    }
}
