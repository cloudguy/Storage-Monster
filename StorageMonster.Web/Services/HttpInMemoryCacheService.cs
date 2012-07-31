using System;
using System.Web;
using StorageMonster.Common;

namespace StorageMonster.Web.Services
{
	internal class HttpInMemoryCacheService : ICacheService
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
	}
}
