using System;
using System.Configuration;
using System.Linq;

namespace StorageMonster.Database.PgSql
{
    public class JustOneDbConfiguration : DbConfigurationBase
    {
        public JustOneDbConfiguration()
            : base("JUSTONEDB_DBI_URL")
        {
        }
        protected override string GetConnectionString()
        {
            var uriString = ConfigurationManager.AppSettings[ConnectionKey];
            if (uriString == null)
                throw new MonsterDbException("No connection settings found");
            var uri = new Uri(uriString);
            return string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};Enlist=true;Pooling=false;",
                uri.Host, uri.Port, uri.AbsolutePath.Trim('/'), uri.UserInfo.Split(':').First(),
                uri.UserInfo.Split(':').Last());
        }
    }
}
