using System;
using System.Collections.Generic;
using System.Web.Mvc;
using StorageMonster.Services;
using StructureMap;

namespace StorageMonster.Web.Services
{
    internal sealed class StructureMapIoC : IocContainer
    {
        public static void CreateContainer()
        {
            StructureMapIoC container = new StructureMapIoC();
// ReSharper disable RedundantNameQualifier
            StructureMapIoC.Configure();
            IocContainer.InstanceInternal = container;
// ReSharper restore RedundantNameQualifier
        }

        private static void Configure()
        {
            ObjectFactory.Initialize(container =>
            {
                container.PullConfigurationFromAppConfig = true;
                container.Scan(x =>
                {
                    x.AssembliesFromApplicationBaseDirectory();
                    x.AddAllTypesOf<Controller>();
                });

                container.For(typeof(Controller)).HttpContextScoped();
            });
        }

        public override T Resolve<T>()
        {
            return ObjectFactory.Container.GetInstance<T>();
        }

        public override object Resolve(Type type)
        {
            return ObjectFactory.Container.GetInstance(type);
        }

        public override string GetLastError()
        {
            return ObjectFactory.WhatDoIHave();
        }

        public override void CleanUpRequestResources()
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }

        public override IEnumerable<T> GetAllInstances<T>()
        {
            return ObjectFactory.GetAllInstances<T>();
        }
    }
}