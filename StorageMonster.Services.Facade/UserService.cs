using System;
using System.Collections.Generic;
using System.Globalization;
using StorageMonster.Common;
using StorageMonster.Database;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Services.Facade
{
	public class UserService : IUserService
	{
	    protected IUserRepository UserRepository { get; set; }
        protected IUserRoleRepository UserRoleRepository { get; set; }

		public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
		{
			UserRepository = userRepository;
			UserRoleRepository = userRoleRepository;
		}

		public User GetUserBySessionToken(Session session)
		{
			return UserRepository.GetUserBySessionToken(session);
		}

		public IEnumerable<UserRole> GetRolesForUser(string email)
		{
			return UserRoleRepository.GetRolesForUser(email);
		}
		public IEnumerable<UserRole> GetRolesForUser(int userId)
		{
			return UserRoleRepository.GetRolesForUser(userId);
		}
		public IEnumerable<UserRole> GetRolesForUser(User user)
		{
			return UserRoleRepository.GetRolesForUser(user);
		}
		public bool IsUserInRole(string userName, string roleName)
		{
			return UserRoleRepository.IsUserInRole(userName, roleName);
		}
		public User GetUserByEmail(string email)
		{
			return UserRepository.GetUserByEmail(email);
		}
        public User Load(int id)
		{
			return UserRepository.Load(id);
		}
        public User Insert(User user)
        {
            return UserRepository.Insert(user);
        }
        public void CreateRoleForUser(User user, string role)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (role == null)
                throw new ArgumentNullException("role");

            UpdateResult updateResult = UserRoleRepository.CreateRoleForUser(user, role);
            switch (updateResult)
            {
                case UpdateResult.Stalled:
                    throw new StaleObjectException(string.Format(CultureInfo.InvariantCulture, "Error creating user role for user with email {0}, object stalled", user.Email));
                case UpdateResult.ItemNotExists:
                    throw new ObjectNotExistsException(string.Format(CultureInfo.InvariantCulture, "Error creating user role for user with email {0}, user not found", user.Email));
            }
        }
        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            UpdateResult updateResult = UserRepository.Update(user);
            switch (updateResult)
            {
                case UpdateResult.Stalled:
                    throw new StaleObjectException(string.Format(CultureInfo.InvariantCulture, "Error updating user {0}, object stalled", user.Id));
                case UpdateResult.ItemNotExists:
                    throw new ObjectNotExistsException(string.Format(CultureInfo.InvariantCulture, "Error updating user {0}, user not found", user.Id));
            }
        }
	}
}
