﻿using System.Data.Common;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace StorageMonster.Utilities
{
    public static class RequestContext
    {
        public static void SetValue(string key, object value)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[key] = value;
            else
                CallContext.SetData(key, value);
        }

        public static object GetValue(string key)
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.Items[key];

            return CallContext.GetData(key);
        }

        public static T GetValue<T>(string key)
        {
            if (HttpContext.Current != null)
                return (T)HttpContext.Current.Items[key];

            return (T)CallContext.GetData(key);
        }
    }
}
