using System.Globalization;
using System.Resources;

namespace CloudBin.Web.Core.Theming
{
    public class Theme
    {
        private readonly ResourceManager _resourceManager;
        private readonly string _name;
        private readonly string _themeBundleName;
        private readonly string _themeDisplayNameKey;
        private readonly bool _isDefault;

        public Theme(string name, bool isDefault, ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            _name = name;
            _themeBundleName = string.Format(CultureInfo.InvariantCulture, "theme_{0}", _name);
            _themeDisplayNameKey = string.Format(CultureInfo.InvariantCulture, "theme_{0}", _name);
            _isDefault = isDefault;
        }

        public string DisplayName
        {
            get { return _resourceManager.GetString(_themeDisplayNameKey); }
        }

        public string BundleName
        {
            get { return _themeBundleName; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool IsDefault
        {
            get { return _isDefault; }
        }
    }
}
