using System;
using System.Collections.Generic;
using StorageMonster.DB.Domain;

namespace StorageMonster.DB.Repositories
{
    public interface IUserRoleRepository
    {
        IEnumerable<UserRole> GetRolesForUser(string email);
        IEnumerable<UserRole> GetRolesForUser(int userId);
        IEnumerable<UserRole> GetRolesForUser(User user);
        bool IsUserInRole(string userName, string roleName);
        UpdateResult CreateRoleForUser(string userEmail, string role, DateTime stamp);
        UpdateResult CreateRoleForUser(User user, string role);
    }
}
