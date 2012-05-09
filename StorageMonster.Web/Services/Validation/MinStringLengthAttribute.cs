using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Globalization;

namespace StorageMonster.Web.Services.Validation
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class MinStringLengthAttribute : ValidationAttribute
	{
		protected const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
		protected readonly int MinCharactersProtected = Membership.Provider.MinRequiredPasswordLength;

        protected readonly PropertyInfo NamePropertyProtected;
        protected readonly Type ResourceTypeProtected;

        public MinStringLengthAttribute(string displayNameKey, Type displayResourceType)
            : base(DefaultErrorMessage)
		{
			ResourceTypeProtected = displayResourceType;
			NamePropertyProtected = ResourceTypeProtected.GetProperty(displayNameKey, BindingFlags.Static | BindingFlags.Public);
		}


        public int MinCharacters { get { return MinCharactersProtected; } }

		public override string FormatErrorMessage(string name)
		{			
			if (NamePropertyProtected == null)
                return String.Format(CultureInfo.CurrentCulture, DefaultErrorMessage, name, MinCharactersProtected);

			string format = (string)NamePropertyProtected.GetValue(NamePropertyProtected.DeclaringType, null);
            return String.Format(CultureInfo.CurrentCulture, format, name, MinCharactersProtected);
		}		

		public override bool IsValid(object value)
		{
			string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= MinCharactersProtected);
		}
	}
}
