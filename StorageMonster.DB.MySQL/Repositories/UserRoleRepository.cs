using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StorageMonster.DB.Domain;
using StorageMonster.DB.Repositories;

namespace StorageMonster.DB.MySQL.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        protected IConnectionProvider Connectionprovider;

        protected const string TableName = "user_roles";
        protected const string SelectFieldList = "ur.id AS Id, ur.user_id AS UserId, ur.role AS Role";

        public UserRoleRepository(IConnectionProvider connectionprovider)
        {
            Connectionprovider = connectionprovider;
        }

        public IEnumerable<UserRole> GetRolesForUser(string email)
        {
            return SqlQueryExecutor.Exceute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} AS ur INNER JOIN users u ON ur.user_id = u.id " +
                                                                            "WHERE u.email=@Email;", SelectFieldList, TableName);
                return Connectionprovider.CurrentConnection.Query<UserRole>(query, new { Email = email });
            });
        }

        public IEnumerable<UserRole> GetRolesForUser(int userId)
        {
            return SqlQueryExecutor.Exceute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} ur WHERE ur.user_id=@UserId;", SelectFieldList, TableName);
                return Connectionprovider.CurrentConnection.Query<UserRole>(query, new { UserId = userId });
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
            return SqlQueryExecutor.Exceute(() =>
                {
                    const string query = " select create_role(@Email, @Role, @Stamp);";
                    return (UpdateResult)Connectionprovider.CurrentConnection.Query<int>(query, new { Email = userEmail, Role = role, Stamp = stamp }).FirstOrDefault();
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
            return SqlQueryExecutor.Exceute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} AS ur INNER JOIN users u ON ur user_id = u.id " +
                                                                            "WHERE u.email=@Email AND ur.role=@Role LIMIT 1;", SelectFieldList, TableName);
                var roles = Connectionprovider.CurrentConnection.Query<UserRole>(query, new { Email = email, Role = roleName });
                return roles != null && roles.FirstOrDefault() != null;
            });
        }
    }
}
