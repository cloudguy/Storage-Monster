using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using System.Globalization;

namespace StorageMonster.Database.MySql.Repositories
{
    public class ResetPasswdRequestsRepository : IResetPasswdRequestsRepository
    {
        protected IConnectionProvider ConnectionProvider { get; set; }

        protected const string TableName = "reset_password_requests";
        protected const string SelectFieldList = "rp.id AS Id, rp.user_id AS UserId, rp.token AS Token, rp.expiration_date AS Expiration";
        protected const string InsertFieldList = "(user_id, token, expiration_date) VALUES (@UserId, @Token, @Expiration)";

        public ResetPasswdRequestsRepository(IConnectionProvider connectionProvider)
        {
            ConnectionProvider = connectionProvider;
        }

        public ResetPasswordRequest GetActiveRequestByToken(string token)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} rp " +
                "WHERE rp.token = @Token AND rp.expiration_date > @Expiration ORDER BY rp.expiration_date DESC LIMIT 1", SelectFieldList, TableName);
                return ConnectionProvider.CurrentConnection.Query<ResetPasswordRequest>(query, new { Token = token, Expiration = DateTime.UtcNow }).FirstOrDefault();
            });
        }

        public ResetPasswordRequest CreateRequest(ResetPasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {1} {0}; SELECT LAST_INSERT_ID();", InsertFieldList, TableName);
                int insertedId = (int)ConnectionProvider.CurrentConnection.Query<long>(query, new { request.UserId, request.Token, request.Expiration }).FirstOrDefault();

                request.Id = insertedId;               
                return request;
            });
        }


        public void DeleteRequest(int id)
        {
            SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0} WHERE id = @Id ",TableName);
                ConnectionProvider.CurrentConnection.Execute(query, new {Id = id });
            });
        }

        public void DeleteExpiredRequests()
        {
            SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0} WHERE expiration_date < @Expiration", TableName);
                ConnectionProvider.CurrentConnection.Execute(query, new { Expiration = DateTime.UtcNow });
            });
        }
    }
}
