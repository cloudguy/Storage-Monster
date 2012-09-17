using System;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Globalization;
using StorageMonster.Services;
using StorageMonster.Web.Services.Security;

namespace StorageMonster.Web.Services.Validation
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinPasswordLengthAttribute : ValidationAttribute
	{
		private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _inCharacters;

        private readonly PropertyInfo _namePropertyProtected;
        private readonly Type _resourceTypeProtected;

        public bool CanBeNull { get; set; }

        public MinPasswordLengthAttribute(string displayNameKey, Type displayResourceType)
            : base(DefaultErrorMessage)
		{
			_resourceTypeProtected = displayResourceType;
			_namePropertyProtected = _resourceTypeProtected.GetProperty(displayNameKey, BindingFlags.Static | BindingFlags.Public);
            _inCharacters = IocContainer.Instance.Resolve<IMembershipService>().MinPasswordLength;
		}


        public int MinCharacters { get { return _inCharacters; } }

		public override string FormatErrorMessage(string name)
		{			
			if (_namePropertyProtected == null)
                return String.Format(CultureInfo.CurrentCulture, DefaultErrorMessage, name, _inCharacters);

			string format = (string)_namePropertyProtected.GetValue(_namePropertyProtected.DeclaringType, null);
            return String.Format(CultureInfo.CurrentCulture, format, name, _inCharacters);
		}		

		public override bool IsValid(object value)
		{
			string valueAsString = value as string;
            if (valueAsString == null)
                return CanBeNull;
            
            return (valueAsString != null && valueAsString.Length >= _inCharacters);
		}
	}
}
