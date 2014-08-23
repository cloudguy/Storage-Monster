using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBin.Web.Core.Configuration
{
    public sealed class ThemeDescription
    {
        public ThemeDescription(string name, bool isDefault)
        {
            Name = name;
            IsDefault = isDefault;
        }
        public string Name { get; private set; }
        public bool IsDefault { get; private set; }
    }
}
