using System;
using System.Collections.Generic;
using System.Web.Mvc;
using StorageMonster.Plugin;
using StructureMap;

namespace StorageMonster.Services
{
    internal sealed class StructureMapIoC : IoCcontainer
    {
        internal void Configure(string configFile)
        {
            ObjectFactory.Initialize(container =>
                {
                    container.AddConfigurationFromXmlFile(configFile);
                    container.Scan(x =>
                        {
                            x.AssembliesFromApplicationBaseDirectory();
                            x.AddAllTypesOf<Controller>();
                        });

                    container.For(typeof (Controller)).HttpContextScoped();
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

