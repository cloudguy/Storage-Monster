using StorageMonster.Database;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace StorageMonster.Services.Facade
{
	public class UserService : IUserService
	{
	    private readonly IUserRepository _userRepository;
	    private readonly IUserRoleRepository _userRoleRepository;
	    private readonly IResetPasswordRequestsRepository _resetPasswdRequestsRepository;

		public UserService(IUserRepository userRepository, 
            IUserRoleRepository userRoleRepository,
            IResetPasswordRequestsRepository resetPasswordRequestsRepository)
		{
			_userRepository = userRepository;
			_userRoleRepository = userRoleRepository;
            _resetPasswdRequestsRepository = resetPasswordRequestsRepository;
		}

		public User GetUserBySessionToken(Session session)
		{
			return _userRepository.GetUserBySessionToken(session);
		}

		public IEnumerable<UserRole> GetRolesForUser(string email)
		{
			return _userRoleRepository.GetRolesForUser(email);
		}
		public IEnumerable<UserRole> GetRolesForUser(int userId)
		{
			return _userRoleRepository.GetRolesForUser(userId);
		}
		public IEnumerable<UserRole> GetRolesForUser(User user)
		{
			return _userRoleRepository.GetRolesForUser(user);
		}
		public bool IsUserInRole(string userName, string roleName)
		{
			return _userRoleRepository.IsUserInRole(userName, roleName);
		}
		public User GetUserByEmail(string email)
		{
			return _userRepository.GetUserByEmail(email);
		}
        public User Load(int id)
		{
			return _userRepository.Load(id);
		}
        public User Insert(User user)
        {
            return _userRepository.Insert(user);
        }
        public void CreateRoleForUser(User user, string role)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (role == null)
                throw new ArgumentNullException("role");

            UpdateResult updateResult = _userRoleRepository.CreateRoleForUser(user, role);
            switch (updateResult)
            {
                case UpdateResult.Stalled:
                    throw new StaleUserException(string.Format(CultureInfo.InvariantCulture, "Error creating user role for user with email {0}, object stalled", user.Email));
                case UpdateResult.ItemNotExists:
                    throw new UserNotExistsException(string.Format(CultureInfo.InvariantCulture, "Error creating user role for user with email {0}, user not found", user.Email));
            }
        }
        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            UpdateResult updateResult = _userRepository.Update(user);
            switch (updateResult)
            {
                case UpdateResult.Stalled:
                    throw new StaleUserException(user.Id);
                case UpdateResult.ItemNotExists:
                    throw new UserNotExistsException(user.Id);
            }
        }       

        public ResetPasswordRequest CreatePasswordResetRequestForUser(User user, DateTime expiration)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            ResetPasswordRequest request = new ResetPasswordRequest
            {
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Expiration = expiration
            };

            return _resetPasswdRequestsRepository.CreateRequest(request);
        }

        public ResetPasswordRequest GetActivePasswordResetRequestByToken(string token)
        {
            return _resetPasswdRequestsRepository.GetActiveRequestByToken(token);
        }
        public void DeleteResetPasswordRequest(int requestId)
        {
            _resetPasswdRequestsRepository.DeleteRequest(requestId);
        }
	}
}
