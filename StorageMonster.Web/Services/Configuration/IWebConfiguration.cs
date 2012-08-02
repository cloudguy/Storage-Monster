using System;

namespace StorageMonster.Web.Services.Configuration
{
	public interface IWebConfiguration
	{
		TimeSpan AuthenticationExpiration { get; }
		string AuthenticationCookiename { get; }
        string LoginUrl { get; }
        bool AuthenticationSlidingExpiration { get; }
        bool AllowMultipleLogons { get; }
	    bool RunSweeper { get; }
	    TimeSpan SweeperTimeout { get; }
        string RestorePasswordMailFrom { get; }
        TimeSpan ResetPasswordRequestExpiration { get; }
	}
}
