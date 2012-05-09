using System;
using System.Globalization;
using System.Linq;
using StorageMonster.DB.Domain;
using StorageMonster.DB.Repositories;

namespace StorageMonster.DB.MySQL.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        protected IConnectionProvider Connectionprovider;
        protected const string TableName = "sessions";
        protected const string SelectFieldList = "id AS Id, user_id AS UserId, session_token AS SessionToken, session_antiforgery_token AS SessionAntiforgeryToken, expiration_date AS Expiration";
        protected const string InsertFieldList = "(user_id, session_token, session_antiforgery_token, expiration_date) VALUES (@UserId, @SessionToken, @SessionAntiforgeryToken, @Expiration)";

        public SessionRepository(IConnectionProvider connectionprovider)
        {
            Connectionprovider = connectionprovider;
        }

        public Session CreateSession(Session session)
        {
            return SqlQueryExecutor.Exceute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {1} {0}; SELECT LAST_INSERT_ID();", InsertFieldList, TableName);
                    int insertedId = (int)Connectionprovider.CurrentConnection.Query<long>(query, new { session.UserId, session.SessionToken, session.SessionAntiforgeryToken, session.Expiration }).FirstOrDefault();
                    if (insertedId <= 0)
                        throw new StorageMonsterDbException("Session insertion failed");

                    session.Id = insertedId;
                    return session;
                });
        }

        public Session GetSessionByToken(string token)
        {
            return SqlQueryExecutor.Exceute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} WHERE session_token=@SessionToken LIMIT 1;", SelectFieldList, TableName);
                Session session = Connectionprovider.CurrentConnection.Query<Session>(query, new { SessionToken = token }).FirstOrDefault();
                return session;
            });
        }

    }
}

