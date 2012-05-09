using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Common;
using System.Web;

namespace StorageMonster.Services
{
	public class InMemoryCache : ICacheService
	{
		public T Get<T>(string cacheID, Func<T> getItemCallback) where T : class
		{
			T item = HttpRuntime.Cache.Get(cacheID) as T;
			if (item == null)
			{
				item = getItemCallback();
				HttpContext.Current.Cache.Insert(cacheID, item);
			}
			return item;
		}
	}

}
