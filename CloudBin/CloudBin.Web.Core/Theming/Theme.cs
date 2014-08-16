using System.Resources;

namespace CloudBin.Web.Core.Theming
{
    public class Theme
    {
        private readonly ResourceManager _resourceManager;
        private readonly string _name;

        public Theme(string name, ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            _name = name;
        }

        public string DisplayName
        {
            get { return _resourceManager.GetString("theme_" + _name); }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
