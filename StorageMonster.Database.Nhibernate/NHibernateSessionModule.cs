using System;
using System.Web;

namespace StorageMonster.Database.Nhibernate
{
    public class NHibernateSessionModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OpenSession;
            context.EndRequest += CloseSession;
        }

        private void OpenSession(object sender, EventArgs e)
        {
            SessionManager.Instance.OpenSession();
        }


        private void CloseSession(object sender, EventArgs e)
        {
            SessionManager.Instance.CloseSession();
        }
      
        public void Dispose() { }
    }
}
