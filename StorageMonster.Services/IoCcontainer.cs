using System;
using System.Collections.Generic;
using StorageMonster.Plugin;

namespace StorageMonster.Services
{
	public abstract class IoCcontainer
	{
        protected static IoCcontainer InstanceInternal;

        public static IoCcontainer Instance
        {
            get { return InstanceInternal; }
        }

        public static void ConfigureStructureMap(string configFile)
        {
            StructureMapIoC container = new StructureMapIoC();
            container.Configure(configFile);
            InstanceInternal = container;
        }

        public abstract T Resolve<T>();

	    public abstract object Resolve(Type type);

	    public abstract string GetLastError();

        public abstract void CleanUpRequestResources();

	    public abstract IEnumerable<T> GetAllInstances<T>();
	}
}

