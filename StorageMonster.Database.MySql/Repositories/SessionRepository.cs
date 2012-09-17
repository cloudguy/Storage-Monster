using System;
using System.Globalization;
using System.Linq;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.MySql.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private const string TableName = "sessions";
        private const string SelectFieldList = "id AS Id, user_id AS UserId, session_token AS Token, expiration_date AS Expiration";
        private const string InsertFieldList = "(user_id, session_token, expiration_date) VALUES (@UserId, @Token, @Expiration)";
		

        public SessionRepository(IConnectionProvider connectionprovider)
        {
            _connectionProvider = connectionprovider;
        }

        public Session CreateSession(Session session)
        {
            return SqlQueryExecutor.Execute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {1} {0}; SELECT LAST_INSERT_ID();", InsertFieldList, TableName);
                    int insertedId = (int)_connectionProvider.CurrentConnection.Query<long>(query, new { session.UserId, session.Token, session.Expiration }).FirstOrDefault();
                    if (insertedId <= 0)
                        throw new MonsterDbException("Session insertion failed");

                    session.Id = insertedId;
                    return session;
                });
        }

        public Session GetSessionByToken(string token)
        {
            return SqlQueryExecutor.Execute(() =>
            {
				String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} WHERE session_token=@Token AND (expiration_date IS NULL OR expiration_date>@Expiration) LIMIT 1;", SelectFieldList, TableName);
				Session session = _connectionProvider.CurrentConnection.Query<Session>(query, new { Token = token, Expiration = DateTime.UtcNow }).FirstOrDefault();
                return session;
            });
        }


		public Session UpdateExpiration(Session session)
		{
			if (session == null)
				throw new ArgumentNullException("session");


            return SqlQueryExecutor.Execute(() =>
			{
				String query = string.Format(CultureInfo.InvariantCulture, "UPDATE {0} SET expiration_date=@Expiration WHERE Id=@Id OR session_token=@Token", TableName);
				_connectionProvider.CurrentConnection.Execute(query, new { session.Id, session.Token, session.Expiration });

				return session;
			});
		}

        public void ClearUserSessions(int userId)
        {
            SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0} WHERE user_id=@UserId", TableName);
                _connectionProvider.CurrentConnection.Execute(query, new { UserId = userId });
            });
        }

        public void ClearExpiredSessions()
        {
            SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0} WHERE expiration_date<@Expiration", TableName);
                _connectionProvider.CurrentConnection.Execute(query, new { Expiration = DateTime.UtcNow });
            });
        }
    }
}

