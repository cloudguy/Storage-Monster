using CloudBin.Core.Configuration;
using CloudBin.Core.Utilities;
using System;
using System.Globalization;

namespace CloudBin.Core
{
    public static class DependencyContainerFactory
    {
        public static IDependencyContainer CreateContainer(IDependencyContainerConfiguration dependencyContainerConfiguration)
        {
            Type type = Type.GetType(dependencyContainerConfiguration.DependencyContainerType);
            Verify.NotNull(() => type, string.Format(CultureInfo.InvariantCulture, "Dependency container type not found: {0}", dependencyContainerConfiguration.DependencyContainerType ?? string.Empty));
            // ReSharper disable AssignNullToNotNullAttribute
            return (IDependencyContainer)Activator.CreateInstance(type);
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}
