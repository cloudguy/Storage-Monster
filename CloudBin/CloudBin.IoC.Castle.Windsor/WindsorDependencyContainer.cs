using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CloudBin.Core;

namespace CloudBin.IoC.Castle.Windsor
{
    public sealed class WindsorDependencyContainer : IDependencyContainer
    {
        private readonly IWindsorContainer _windsorContainer;

        private WindsorDependencyContainer()
        {
            _windsorContainer = new WindsorContainer();
        }

        public static IDependencyContainer Create()
        {
            return new WindsorDependencyContainer();
        }

        #region IDependencyContainer implementation

        IDependencyContainer IDependencyContainer.RegisterTypesInDirectory(Type baseType, string directory)
        {
            _windsorContainer.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter(directory)).BasedOn(baseType).LifestyleTransient());
            return this;
        }

        IDependencyContainer IDependencyContainer.RegisterFromAppConfig()
        {
            _windsorContainer.Install(Configuration.FromAppConfig());
            return this;
        }

        T IDependencyContainer.Resolve<T>()
        {
            return _windsorContainer.Resolve<T>();
        }

        object IDependencyContainer.Resolve(Type serviceType)
        {
            return _windsorContainer.Resolve(serviceType);
        }

        IEnumerable<T> IDependencyContainer.ResolveAll<T>()
        {
            return _windsorContainer.ResolveAll<T>();
        }

        IEnumerable<object> IDependencyContainer.ResolveAll(Type serviceType)
        {
            return _windsorContainer.ResolveAll(serviceType).OfType<object>();
        }

        bool IDependencyContainer.HasService<T>()
        {
            return _windsorContainer.Kernel.HasComponent(typeof (T));
        }

        bool IDependencyContainer.HasService(Type serviceType)
        {
            return _windsorContainer.Kernel.HasComponent(serviceType);
        }

        void IDependencyContainer.Release(object instance)
        {
            _windsorContainer.Release(instance);
        }

        #endregion

        #region IDisposable implementation

        void IDisposable.Dispose()
        {
            _windsorContainer.Dispose();
        }

        #endregion
    }
}
