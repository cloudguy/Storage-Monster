using System;
using System.Collections.Generic;

namespace CloudBin.Core
{
    public interface IDependencyContainer : IDisposable
    {
        IDependencyContainer RegisterTypesInDirectory(Type baseType, string directory);
        IDependencyContainer RegisterFromAppConfig();
        T Resolve<T>();
        object Resolve(Type serviceType);
        IEnumerable<T> ResolveAll<T>();
        IEnumerable<object> ResolveAll(Type serviceType);
        bool HasService<T>();
        bool HasService(Type serviceType);
        void Release(object instance);
    }
}
