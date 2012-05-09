using System;

namespace StorageMonster.Services
{
	public abstract class IoCcontainer
	{
        protected static IoCcontainer InstanceInternal;

        public static IoCcontainer Instance
        {
            get { return InstanceInternal; }
        }

        public static void ConfigureStructureMap()
        {
            StructureMapIoC container = new StructureMapIoC();
            container.Configure();
            InstanceInternal = container;
        }

       public static void ConfigureStructureMap(Action configAction)
       {
           StructureMapIoC container = new StructureMapIoC();
           container.Configure(configAction);
           InstanceInternal = container;
       }

        public abstract T Resolve<T>();

	    public abstract object Resolve(Type type);

	    public abstract string GetLastError();

        public abstract void CleanUpRequestResources();
	}
}

