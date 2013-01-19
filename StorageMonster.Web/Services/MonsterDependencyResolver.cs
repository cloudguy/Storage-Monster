﻿using StorageMonster.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StorageMonster.Web.Services
{
    public sealed class MonsterDependencyResolver : IDependencyResolver
    {
        private readonly IocContainer _container;

        public MonsterDependencyResolver(IocContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }
    }
}