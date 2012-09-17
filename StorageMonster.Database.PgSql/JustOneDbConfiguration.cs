using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace StorageMonster.Database.PgSql
{
    public class JustOneDbConfiguration : IDbConfiguration
    {
        private const string ConnectionKey = "JUSTONEDB_DBI_URL";
        private static string _connectionString;
        private static readonly object _locker = new object();

        public string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    lock (_locker)
                    {
                        if (_connectionString == null)
                        {
                            var uriString = ConfigurationManager.AppSettings[ConnectionKey];
                            var uri = new Uri(uriString);
                            _connectionString = string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};Enlist=true;Pooling=false;",
                                uri.Host, uri.Port, uri.AbsolutePath.Trim('/'), uri.UserInfo.Split(':').First(),
                                uri.UserInfo.Split(':').Last());
                        }
                    }
                }
                return _connectionString;
            }
        }
    }
}
