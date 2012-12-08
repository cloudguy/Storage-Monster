using Npgsql;
using System.Data;
using System.Data.Common;

namespace StorageMonster.Database.PgSql
{
    public class ConnectionProvider : ConnectionProviderBase
    {
        public ConnectionProvider(IDbConfiguration dbConfiguration)
            :base(dbConfiguration)
        {
        }

        public override DbConnection CreateConnection()
        {  
            DbConnection connection = new NpgsqlConnection(DbConfiguration.ConnectionString);
           
            connection.Open();           
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SET TIME ZONE 'UTC';";                
                command.ExecuteNonQuery();
                command.CommandText = "SET CLIENT_ENCODING TO 'UTF8';";
                command.ExecuteNonQuery();
            }
            return connection;
        }
    }
}