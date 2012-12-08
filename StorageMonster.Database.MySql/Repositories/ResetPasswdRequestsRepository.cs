using System;
using System.Globalization;
using System.Linq;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using StorageMonster.MicroORM;

namespace StorageMonster.Database.MySql.Repositories
{
    public class ResetPasswordRequestsRepository : IResetPasswordRequestsRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        private const string TableName = "reset_password_requests";
        private const string SelectFieldList = "rp.id AS Id, rp.user_id AS UserId, rp.token AS Token, rp.expiration_date AS Expiration";
        private const string InsertFieldList = "(user_id, token, expiration_date) VALUES (@UserId, @Token, @Expiration)";

        public ResetPasswordRequestsRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public ResetPasswordRequest GetActiveRequestByToken(string token)
        {
            String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} rp " +
                                                                       "WHERE rp.token = @Token AND rp.expiration_date > @Expiration ORDER BY rp.expiration_date DESC LIMIT 1", SelectFieldList, TableName);
            return _connectionProvider.CurrentConnection.Query<ResetPasswordRequest>(query, new {Token = token, Expiration = DateTime.UtcNow}).FirstOrDefault();

        }

        public ResetPasswordRequest CreateRequest(ResetPasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            String query = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {1} {0}; SELECT LAST_INSERT_ID();", InsertFieldList, TableName);
            int insertedId = (int) _connectionProvider.CurrentConnection.Query<long>(query, new {request.UserId, request.Token, request.Expiration}).FirstOrDefault();

            request.Id = insertedId;
            return request;
        }


        public void DeleteRequest(int id)
        {
            String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0} WHERE id = @Id ", TableName);
            _connectionProvider.CurrentConnection.Execute(query, new {Id = id});
        }

        public void DeleteExpiredRequests()
        {
            String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0} WHERE expiration_date < @Expiration", TableName);
            _connectionProvider.CurrentConnection.Execute(query, new {Expiration = DateTime.UtcNow});
        }
    }
}
