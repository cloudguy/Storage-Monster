using System;

namespace StorageMonster.Web.Services.Configuration
{
	public interface IWebConfiguration
	{
		TimeSpan AuthenticationExpiration { get; }
		string AuthenticationCookieName { get; }
        string LocaleCookieName { get; }
        TimeSpan LocaleCookieTimeout { get; }
	    int MinPasswordLength { get; }        
        bool AuthenticationSlidingExpiration { get; }
        bool AllowMultipleLogons { get; }
	    bool RunSweeper { get; }
	    TimeSpan SweeperTimeout { get; }
        string RestorePasswordMailFrom { get; }
        TimeSpan ResetPasswordRequestExpiration { get; }
        string SiteUrl { get; }
	    bool AutoDetectSiteUrl { get; }
	}
}
