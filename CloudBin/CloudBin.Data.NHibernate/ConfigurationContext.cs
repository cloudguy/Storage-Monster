using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CloudBin.Core.Utilities;

namespace CloudBin.Data.NHibernate
{
    internal class ConfigurationContext
    {
        private static readonly Synchronized<ConfigurationContext> CurrentContext = new Synchronized<ConfigurationContext>();

        internal IDatabaseConfiguration Configuration { get; private set; }

        internal ConfigurationContext(IDatabaseConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal static void SetContext(ConfigurationContext context)
        {
            CurrentContext.Value = context;
        }

        internal static void ResetContext()
        {
            CurrentContext.Value = null;
        }

        internal static ConfigurationContext Current
        {
            get
            {
                return CurrentContext.Value;
            }
        }
    }
}
