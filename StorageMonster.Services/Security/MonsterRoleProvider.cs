using System;
using System.Collections.Generic;
using System.Linq;
using StorageMonster.DB.Repositories;

namespace StorageMonster.Services.Security
{
    public class MonsterRoleProvider : System.Web.Security.RoleProvider
    {
        public const string RoleUser = "ROLE_USER";
        public const string RoleAdmin = "ROLE_ADMIN";

        protected static IEnumerable<String> AvailableRoles; 

        static MonsterRoleProvider()
        {
            AvailableRoles = new List<String> { RoleUser, RoleAdmin };
        }

        public override string[] GetRolesForUser(string useremail)
        {
            var userRoleRepo = IoCcontainer.Instance.Resolve<IUserRoleRepository>();
            var roles = userRoleRepo.GetRolesForUser(useremail);
            return roles.Select(r => r.Role).ToArray();
        }

        public override bool IsUserInRole(string userEmail, string roleName)
        {
            var userRoleRepo = IoCcontainer.Instance.Resolve<IUserRoleRepository>();
            return userRoleRepo.IsUserInRole(userEmail, roleName);
        }

        public override string[] GetAllRoles()
        {
            return AvailableRoles.ToArray();
        }

        public override bool RoleExists(string roleName)
        {
            return AvailableRoles.Where(r => string.Compare(r, roleName, StringComparison.OrdinalIgnoreCase) == 0)
                .FirstOrDefault() != null;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
    }
}
