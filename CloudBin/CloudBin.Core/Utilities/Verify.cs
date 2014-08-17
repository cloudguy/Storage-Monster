using System;
using System.Linq.Expressions;

namespace CloudBin.Core.Utilities
{
    public static class Verify
    {
        #region NotNull
        public static void NotNull(object obj, string paramName, string message)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
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
            NotNull(paramExpression, (string)null);
        }

        public static void NotNull<T>(Expression<Func<T>> paramExpression, Func<Exception> exceptionFactory) where T : class
        {
            T value = paramExpression.Compile().Invoke();
            if (value == null)
            {
                Exception ex = exceptionFactory();
                throw ex;
            }
        }

        #endregion

        #region NotNullOrWhiteSpace
        public static void NotNullOrWhiteSpace(string value, string paramName, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static void NotNullOrWhiteSpace(string value, string paramName)
        {
            NotNullOrWhiteSpace(value, paramName, null);
        }

        public static void NotNullOrWhiteSpace(Expression<Func<string>> paramExpression, string message)
        {
            string value = paramExpression.Compile().Invoke();
            NotNullOrWhiteSpace(value, ReflectionHelper.GetParamName(paramExpression), message);
        }

        public static void NotNullOrWhiteSpace(Expression<Func<string>> paramExpression)
        {
            NotNullOrWhiteSpace(paramExpression, null);
        }

        #endregion

        public static void ThrowIfNotNull<T, TE>(Expression<Func<T>> paramExpression, Func<TE> exceptionPredicate) where T : class where TE : Exception
        {
            T value = paramExpression.Compile().Invoke();
            if (value != null)
            {
                throw exceptionPredicate();
            }
        }
    }
}
