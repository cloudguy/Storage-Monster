using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StorageMonster.Utilities
{
    public static class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(object instance, Type attributeType)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Type instanseType = instance.GetType();

            return from prop in instanseType.GetProperties(BindingFlags.Public | BindingFlags.Instance) 
                   let attrs = prop.GetCustomAttributes(attributeType, true) 
                   where attrs.Length > 0 
                   select prop;
        }
    }
}
