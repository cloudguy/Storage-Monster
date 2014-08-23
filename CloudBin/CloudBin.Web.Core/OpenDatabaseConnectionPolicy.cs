using System.Collections.Generic;
using CloudBin.Core;
using CloudBin.Web.Core.Configuration;
using System;
using System.Linq;
using System.Web;

namespace CloudBin.Web.Core
{
#warning remove this - make db session lazy
    public sealed class OpenDatabaseConnectionPolicy : IOpenDatabaseConnectionPolicy
    {
        private readonly IWebConfiguration _webConfiguration;
        private readonly static object Locker = new object();
        private volatile static Func<HttpContext, bool>[] _policyCheckerInternal;

        private IEnumerable<Func<HttpContext, bool>> PolicyCheckers
        {
            get
            {
                if (_policyCheckerInternal == null)
                {
                    lock (Locker)
                    {
                        if (_policyCheckerInternal == null)
                        {
                            _policyCheckerInternal = RequestCheckersFactory.CreateStaticContentCheckers(HttpContext.Current);
                        }
                    }
                }
                return _policyCheckerInternal;
            }
        }

        public OpenDatabaseConnectionPolicy(IWebConfiguration webConfiguration)
        {
            _webConfiguration = webConfiguration;
        }
        bool IOpenDatabaseConnectionPolicy.DatabaseConnectionRequired(HttpContext context)
        {   
            if (!_webConfiguration.DoNotOpenDbSessionForScriptAndContent)
            {
                return true;
            }
            return RequestContext.Current.LookUpValue("db_open_required", () => !PolicyCheckers.Any(checker => checker(context)));
        }
    }
}