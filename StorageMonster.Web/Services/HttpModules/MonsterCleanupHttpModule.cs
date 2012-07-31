using System;
using System.Web;
using StorageMonster.Database;
using StorageMonster.Services;


namespace StorageMonster.Web.Services.HttpModules
{
	public class MonsterCleanupHttpModule : IHttpModule
	{
		public void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
            if (context == null)
                throw new ArgumentNullException("context");

            context.EndRequest += application_EndRequest;
		}

// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable InconsistentNaming
		void application_EndRequest(object sender, EventArgs e)
// ReSharper restore InconsistentNaming
// ReSharper restore MemberCanBeMadeStatic.Local
		{
			IConnectionProvider connectionProvider = IocContainer.Instance.Resolve<IConnectionProvider>();
			connectionProvider.CloseCurrentConnection();
			IocContainer.Instance.CleanUpRequestResources();
		}
	}
}
