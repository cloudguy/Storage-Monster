using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.MySql.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        protected IConnectionProvider ConnectionProvider { get; set; }

        protected const string TableName = "user_roles";
        protected const string SelectFieldList = "ur.id AS Id, ur.user_id AS UserId, ur.role AS Role";
        protected const string InsertFieldList = "(user_id, role) VALUES (@UserId, @Role)";
        

        public UserRoleRepository(IConnectionProvider connectionprovider)
        {
            ConnectionProvider = connectionprovider;
        }

        public IEnumerable<UserRole> GetRolesForUser(string email)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} AS ur INNER JOIN users u ON ur.user_id = u.id " +
                                                                            "WHERE u.email=@Email;", SelectFieldList, TableName);
                return ConnectionProvider.CurrentConnection.Query<UserRole>(query, new { Email = email });
            });
        }

        public IEnumerable<UserRole> GetRolesForUser(int userId)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} ur WHERE ur.user_id=@UserId;", SelectFieldList, TableName);
                return ConnectionProvider.CurrentConnection.Query<UserRole>(query, new { UserId = userId });
            });
        }

        public IEnumerable<UserRole> GetRolesForUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return GetRolesForUser(user.Id);
        }

        public UpdateResult CreateRoleForUser(string userEmail, string role, DateTime stamp)
        {
            return SqlQueryExecutor.Execute(() =>
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        UpdateResult result = UpdateResult.Success;
                        const string selectQuery = "SELECT count(ur.id) FROM user_roles ur " +
                                                   "INNER JOIN users u ON ur.user_id = u.id " +
                                                   "WHERE u.email = @Email AND ur.role = @Role AND u.stamp = @Stamp;";

                        long rolesCount = ConnectionProvider.CurrentConnection.Query<long>(selectQuery, new { Email = userEmail, Role = role, Stamp = stamp }).FirstOrDefault();

                        if (rolesCount <= 0)
                        {
                            const string selectUserQueryWithStamp = "SELECT u.id FROM users u WHERE u.email = @Email AND u.stamp = @Stamp LIMIT 1;";
                            int? userId = ConnectionProvider.CurrentConnection.Query<int?>(selectUserQueryWithStamp, new { Email = userEmail, Stamp = stamp }).FirstOrDefault();
                            if (userId == null || userId.Value <= 0)
                            {//user does not exists or was updated
                                const string selectUserQuery = "SELECT u.id FROM users u WHERE u.email = @Email LIMIT 1;";
                                userId = ConnectionProvider.CurrentConnection.Query<int?>(selectUserQuery, new { Email = userEmail }).FirstOrDefault();
                                if (userId == null || userId.Value <= 0)
                                    result = UpdateResult.ItemNotExists; //user does not exists
                                else
                                    result = UpdateResult.Stalled; //user stalled
                            }
                            else
                            {
                                string insertQuery = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {0} {1}; UPDATE users SET stamp = CURRENT_TIMESTAMP() WHERE id = @UserId;", TableName, InsertFieldList);
                                ConnectionProvider.CurrentConnection.Execute(insertQuery, new { UserId = userId.Value, Role = role });
                                scope.Complete();
                            }
                        }
                        return result;
                    }
                });
        }

        public UpdateResult CreateRoleForUser(User user, string role)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return CreateRoleForUser(user.Email, role, user.Stamp);
        }

        public bool IsUserInRole(string email, string roleName)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} AS ur INNER JOIN users u ON ur user_id = u.id " +
                                                                            "WHERE u.email=@Email AND ur.role=@Role LIMIT 1;", SelectFieldList, TableName);
                var roles = ConnectionProvider.CurrentConnection.Query<UserRole>(query, new { Email = email, Role = roleName });
                return roles != null && roles.FirstOrDefault() != null;
            });
        }
    }
}
