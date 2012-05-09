using System;
using System.Reflection;
using System.Globalization;

namespace StorageMonster.Util
{
	public static class ReflectionHelper
	{
		#region Field
		public static FieldInfo GetField(object instance, string fieldName, BindingFlags bindingFlags)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			if (fieldName == null)
				throw new ArgumentNullException("fieldName");

			Type instanceType = instance.GetType();
			return instanceType.GetField(fieldName, bindingFlags);
		}

		public static FieldInfo GetField(object instance, string fieldName)
		{
			return GetField(instance, fieldName, BindingFlags.Instance | BindingFlags.Public);
		}

		public static object GetFieldValue(object instance, string fieldName, BindingFlags bindingFlags)
		{
			FieldInfo field = GetField(instance, fieldName, bindingFlags);
			if (field == null)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Field '{0}' not found in type {1}", fieldName, instance.GetType().FullName));

			return field.GetValue(instance);
		}

		public static object GetFieldValue(object instance, string fieldName)
		{
			return GetFieldValue(instance, fieldName, BindingFlags.Instance | BindingFlags.Instance);
		}

		public static T GetFieldValue<T>(object instance, string fieldName)
		{
			return (T)GetFieldValue(instance, fieldName);
		}

		public static T GetFieldValue<T>(object instance, string fieldName, BindingFlags bindingFlags)
		{
			return (T)GetFieldValue(instance, fieldName, bindingFlags);
		}
		#endregion

		#region Property
		public static PropertyInfo GetProperty(object instance, string propertyName, BindingFlags bindingFlags)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			if (propertyName == null)
				throw new ArgumentNullException("propertyName");

			Type instanceType = instance.GetType();
			return instanceType.GetProperty(propertyName, bindingFlags);
		}

		public static PropertyInfo GetProperty(object instance, string propertyName)
		{
			return GetProperty(instance, propertyName, BindingFlags.Instance | BindingFlags.Public);
		}

		public static object GetPropertyValue(object instance, string propertyName, BindingFlags bindingFlags)
		{
			PropertyInfo property = GetProperty(instance, propertyName, bindingFlags);
			if (property == null)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Property '{0}' not found in type {1}", propertyName, instance.GetType().FullName));

			return property.GetValue(instance, null);
		}

		public static object GetPropertyValue(object instance, string propertyName)
		{
			return GetPropertyValue(instance, propertyName, BindingFlags.Instance | BindingFlags.Public);
		}

		public static T GetPropertyValue<T>(object instance, string propertyName)
		{
			return (T)GetPropertyValue(instance, propertyName);
		}

		public static T GetPropertyValue<T>(object instance, string propertyName, BindingFlags bindingFlags)
		{
			return (T)GetPropertyValue(instance, propertyName, bindingFlags);
		}
		#endregion


		public static object GetFieldOrPropertyValue(object instance, string propertyOrFieldName, BindingFlags bindingFlags)
		{
			PropertyInfo property = GetProperty(instance, propertyOrFieldName, bindingFlags);
			FieldInfo field;
			if (property == null)
			{
				field = GetField(instance, propertyOrFieldName, bindingFlags);
				if (field == null)
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Neither property nor field with name '{0}' not found in type {1}", propertyOrFieldName, instance.GetType().FullName));

				return field.GetValue(instance);
			}
			return property.GetValue(instance, null);
		}

		public static object GetFieldOrPropertyValue(object instance, string propertyOrFieldName)
		{
			return GetFieldOrPropertyValue(instance, propertyOrFieldName, BindingFlags.Instance | BindingFlags.Public);
		}

		public static T GetFieldOrPropertyValue<T>(object instance, string propertyOrFieldName)
		{
			return (T)GetFieldOrPropertyValue(instance, propertyOrFieldName);
		}

		public static T GetFieldOrPropertyValue<T>(object instance, string propertyOrFieldName, BindingFlags bindingFlags)
		{
			return (T)GetFieldOrPropertyValue(instance, propertyOrFieldName, bindingFlags);
		}

	}
}
