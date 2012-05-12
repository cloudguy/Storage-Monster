using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Common;
using System.Web;

namespace StorageMonster.Services
{
	internal class InMemoryCache : ICacheService
	{
		public T Get<T>(string cacheId, Func<T> getItemCallback) where T : class
		{
			T item = HttpRuntime.Cache.Get(cacheId) as T;
			if (item == null)
			{
				item = getItemCallback();
				HttpContext.Current.Cache.Insert(cacheId, item);
			}
			return item;
		}
        public void Set<T>(string cacheId, T item) where T : class
        {
            HttpContext.Current.Cache.Insert(cacheId, item);
        }
        public T Get<T>(string cacheId) where T : class
        {
            return HttpRuntime.Cache.Get(cacheId) as T;
        }
	}

}
