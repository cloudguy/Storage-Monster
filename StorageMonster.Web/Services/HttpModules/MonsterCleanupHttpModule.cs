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

		void application_EndRequest(object sender, EventArgs e)
		{
			IConnectionProvider connectionProvider = IocContainer.Instance.Resolve<IConnectionProvider>();
			connectionProvider.CloseCurrentConnection();
			IocContainer.Instance.CleanUpRequestResources();
		}
	}
}
