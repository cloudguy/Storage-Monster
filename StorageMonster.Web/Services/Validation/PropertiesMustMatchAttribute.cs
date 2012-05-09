using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using StorageMonster.Util;

namespace StorageMonster.Web.Services.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public class PropertiesMustMatchAttribute : ValidationAttribute
	{
		protected const string DefaultErrorMessage = "Fields do not match";
		
		protected readonly PropertyInfo NameProperty;
		protected readonly Type ResourceType;

        public String PropertyNameToMatch { get; protected set; }

        public PropertiesMustMatchAttribute(string propertyNameToMatch, string displayNameKey, Type displayResourceType)
			: base(DefaultErrorMessage)
		{
            PropertyNameToMatch = propertyNameToMatch;
			ResourceType = displayResourceType;
			NameProperty = ResourceType.GetProperty(displayNameKey, BindingFlags.Static | BindingFlags.Public);
		}		

		public override string FormatErrorMessage(string name)
		{			
			if (NameProperty == null)
				return DefaultErrorMessage;

			return (string)NameProperty.GetValue(NameProperty.DeclaringType, null);			
		}		

		public override bool IsValid(object value)
		{
		    throw new InvalidOperationException("Attribure requires validator");
		}
	}
}
