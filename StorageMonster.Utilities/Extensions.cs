using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Utilities
{
    public static class Extensions
    {
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o);
        }

        public static T With<T>(this T o, Func<T, bool> evaluator)            
            where T : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }
    }
}
