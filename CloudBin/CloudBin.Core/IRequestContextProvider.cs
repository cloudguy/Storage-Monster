using System;

namespace CloudBin.Core
{
    public interface IRequestContextProvider
    {
        void SetValue(string key, object value);
        object GetValue(string key);
        T GetValue<T>(string key);
        T LookUpValue<T>(string key, Func<T> valueProvider);
    }
}
