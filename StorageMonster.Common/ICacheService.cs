using System;

namespace StorageMonster.Common
{
	public interface ICacheService
	{
		T Get<T>(string cacheId, Func<T> getItemCallback) where T : class;
	}
}
