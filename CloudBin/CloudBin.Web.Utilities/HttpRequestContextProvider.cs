using CloudBin.Core;
using CloudBin.Core.Utilities;
using System;
using System.Web;

namespace CloudBin.Web.Utilities
{
    public sealed class HttpRequestContextProvider : IRequestContextProvider
    {
        private void SetValueInternal(string key, object value)
        {
            HttpContext.Current.Items[key] = value;
        }

        private object GetValueInternal(string key)
        {
            return HttpContext.Current.Items[key];
        }

        void IRequestContextProvider.SetValue(string key, object value)
        {
            SetValueInternal(key, value);
        }

        object IRequestContextProvider.GetValue(string key)
        {
            return GetValueInternal(key);
        }

        T IRequestContextProvider.GetValue<T>(string key)
        {
            return (T)GetValueInternal(key);
        }

        T IRequestContextProvider.LookUpValue<T>(string key, Func<T> valueProvider)
        {
            Verify.NotNull(()=>valueProvider);
            object valueAsObject = GetValueInternal(key);
            T value;
            if (valueAsObject == null)
            {
                value = valueProvider();
                SetValueInternal(key, value);
            }
            else
            {
                value = (T)valueAsObject;
            }
            return value;
        }
    }
}
