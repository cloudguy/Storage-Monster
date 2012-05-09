using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Common
{
	public interface ICacheService
	{
		T Get<T>(string cacheID, Func<T> getItemCallback) where T : class;
	}
}
