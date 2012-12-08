using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using StorageMonster.MicroORM;

namespace StorageMonster.Database.MySql.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        private const string TableName = "user_roles";
        private const string SelectFieldList = "ur.id AS Id, ur.user_id AS UserId, ur.role AS Role";
        private const string InsertFieldList = "(user_id, role) VALUES (@UserId, @Role)";


        public UserRoleRepository(IConnectionProvider connectionprovider)
        {
            _connectionProvider = connectionprovider;
        }

        public IEnumerable<UserRole> GetRolesForUser(string email)
        {
            String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} AS ur INNER JOIN users u ON ur.user_id = u.id " +
                                                                       "WHERE u.email=@Email;", SelectFieldList, TableName);
            return _connectionProvider.CurrentConnection.Query<UserRole>(query, new {Email = email});

        }

        public IEnumerable<UserRole> GetRolesForUser(int userId)
        {
            String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} ur WHERE ur.user_id=@UserId;", SelectFieldList, TableName);
            return _connectionProvider.CurrentConnection.Query<UserRole>(query, new {UserId = userId});
        }

        public IEnumerable<UserRole> GetRolesForUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return GetRolesForUser(user.Id);
        }

        public UpdateResult CreateRoleForUser(string userEmail, string role, DateTime stamp)
        {
            return _connectionProvider.DoInTransaction(() =>
                {
                    UpdateResult result = UpdateResult.Success;
                    const string selectUserQuery = "SELECT u.id FROM users u " +
                                                    "WHERE u.email = @Email AND u.stamp = @Stamp LIMIT 1;";

                    int? userId = _connectionProvider.CurrentConnection.Query<int?>(selectUserQuery, new { Email = userEmail, Stamp = stamp }).FirstOrDefault();
                    
                    if (userId == null)
                    {
                        const string selectUserQueryWithStamp = "SELECT u.id FROM users u WHERE u.email = @Email LIMIT 1;";
                        userId = _connectionProvider.CurrentConnection.Query<int?>(selectUserQueryWithStamp, new {Email = userEmail}).FirstOrDefault();
                        result = userId == null ? UpdateResult.ItemNotExists : UpdateResult.Stalled;
                    }
                    else
                    {
                        const string selectRoleQuery = "SELECT ur.id FROM user_roles ur " +
                                                        "INNER JOIN users u ON ur.user_id = u.id " +
                                                        "WHERE u.Id = @Id AND ur.role = @Role";
                        int? roleId = _connectionProvider.CurrentConnection.Query<int?>(selectRoleQuery, new { Id = userId.Value, Role = role }).FirstOrDefault();
                        if (roleId == null)
                        {
                            string insertQuery = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {0} {1}; UPDATE users SET stamp = now() WHERE id = @UserId;", TableName, InsertFieldList);
                            _connectionProvider.CurrentConnection.Execute(insertQuery, new {UserId = userId.Value, Role = role});
                        }
                    }
                    return result;
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
            String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} AS ur INNER JOIN users u ON ur.user_id = u.id " +
                                                                       "WHERE u.email=@Email AND ur.role=@Role LIMIT 1;", SelectFieldList, TableName);
            var roles = _connectionProvider.CurrentConnection.Query<UserRole>(query, new {Email = email, Role = roleName});
            return roles != null && roles.FirstOrDefault() != null;
        }
    }
}
