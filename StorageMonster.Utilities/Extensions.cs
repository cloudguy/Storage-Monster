using System;

namespace StorageMonster.Utilities
{
    public static class Extensions
    {
        public static TResult With<TInput, TResult>(this TInput @object, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (evaluator == null)
                throw new ArgumentNullException("evaluator");

            if (@object == null) return null;
            return evaluator(@object);
        }

        public static T With<T>(this T @object, Func<T, bool> evaluator)            
            where T : class
        {
            if (evaluator == null)
                throw new ArgumentNullException("evaluator");

            if (@object == null) return null;
            return evaluator(@object) ? @object : null;
        }
    }
}
