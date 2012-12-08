using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace StorageMonster.Database.MySql
{
    public class ConnectionProvider : ConnectionProviderBase
    {
        public ConnectionProvider(IDbConfiguration dbConfiguration)
            :base(dbConfiguration)
        {
        }

        public override DbConnection CreateConnection()
        {
            DbConnection connection = new MySqlConnection(DbConfiguration.ConnectionString);

            connection.Open();
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SET time_zone='+00:00'; SET names 'utf8';";
                command.ExecuteNonQuery();
            }
            return connection;
        }
    }
}
