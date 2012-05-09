using System;
using System.Web.Mvc;
using StructureMap;

namespace StorageMonster.Services
{
    internal sealed class StructureMapIoC : IoCcontainer
    {
        internal void Configure(Action configAction)
        {
            configAction();
        }

        internal void Configure()
        {
            ObjectFactory.Initialize(container =>
                {
                    container.AddConfigurationFromXmlFile("IoC.xml");
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
    }
}

