using CloudBin.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CloudBin.Web.Utilities
{
    public sealed class DependencyResolver : IDependencyResolver
    {
        private readonly IDependencyContainer _dependencyContainer;

        public DependencyResolver(IDependencyContainer dependencyContainer)
        {
            _dependencyContainer = dependencyContainer;
        }

        #region IDependencyResolver implementation

        object IDependencyResolver.GetService(Type serviceType)
        {
            if (_dependencyContainer.HasService(serviceType))
            {
                return _dependencyContainer.Resolve(serviceType);
            }
            return null;
        }

        IEnumerable<object> IDependencyResolver.GetServices(Type serviceType)
        {
            return _dependencyContainer.ResolveAll(serviceType);
        }

        #endregion
    }
}
