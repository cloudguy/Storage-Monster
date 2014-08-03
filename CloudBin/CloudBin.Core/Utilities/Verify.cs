using System;
using System.Linq.Expressions;

namespace CloudBin.Core.Utilities
{
    public static class Verify
    {
        public static void NotNull(object obj, string paramName, string message)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName, message);
        }

        public static void NotNull(object obj, string paramName)
        {
            NotNull(obj, paramName, null);
        }

        public static void NotNull<T>(Expression<Func<T>> paramExpression, string message) where T : class
        {
            T value = paramExpression.Compile().Invoke();
            NotNull(value, ReflectionHelper.GetParamName(paramExpression), message);
        }

        public static void NotNull<T>(Expression<Func<T>> paramExpression) where T : class
        {
            NotNull(paramExpression, null);
        }

        public static void ThrowIfNotNull<T, E>(Expression<Func<T>> paramExpression, Func<E> exceptionPredicate) where T : class where E : Exception
        {
            T value = paramExpression.Compile().Invoke();
            if (value != null)
            {
                throw exceptionPredicate();
            }
        }
    }
}
