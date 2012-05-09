using System;
using System.ComponentModel;
using System.Reflection;

namespace StorageMonster.Web.Services
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class LocalizedDisplayNameAttribute : DisplayNameAttribute
	{
		protected readonly PropertyInfo NameProperty;
		protected readonly Type ResourceType;

		public LocalizedDisplayNameAttribute(string displayNameKey, Type displayResourceType)
			: base(displayNameKey)
		{
			ResourceType = displayResourceType;
			NameProperty = ResourceType.GetProperty(base.DisplayName, BindingFlags.Static | BindingFlags.Public);
		}		

		public override string DisplayName
		{
			get
			{				
				if (NameProperty == null)				
					return base.DisplayName;				

				return (string)NameProperty.GetValue(NameProperty.DeclaringType, null);
			}
		}
	}
}
