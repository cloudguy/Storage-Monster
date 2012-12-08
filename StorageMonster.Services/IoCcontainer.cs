using System;
using System.Collections.Generic;

namespace StorageMonster.Services
{
	public abstract class IocContainer
	{
        protected static IocContainer InstanceInternal;

        public static IocContainer Instance
        {
            get { return InstanceInternal; }
        }

        public abstract T Resolve<T>();

	    public abstract object Resolve(Type type);

	    public abstract string GetLastError();

        public abstract void CleanUpRequestResources();

	    public abstract IEnumerable<T> GetAllInstances<T>();

        public abstract IEnumerable<object> GetAllInstances(Type serviceType);
	}
}

