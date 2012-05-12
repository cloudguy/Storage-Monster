using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Common
{
	public interface ICacheService
	{
		T Get<T>(string cacheId, Func<T> getItemCallback) where T : class;
        void Set<T>(string cacheId, T item) where T : class;
        T Get<T>(string cacheId) where T : class;
	}
}
