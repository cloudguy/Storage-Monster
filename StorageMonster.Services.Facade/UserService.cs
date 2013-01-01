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
	    private readonly IResetPasswordRequestsRepository _resetPasswdRequestsRepository;

		public UserService(IUserRepository userRepository,
            IResetPasswordRequestsRepository resetPasswordRequestsRepository)
		{
			_userRepository = userRepository;
            _resetPasswdRequestsRepository = resetPasswordRequestsRepository;
		}
		
        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }
       
        public User Insert(User user)
        {
            return _userRepository.Insert(user);
        }

        public User GetUserBySessionToken(Session session)
        {
            throw new NotImplementedException();
        }

        public User Load(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            throw new NotImplementedException();
        }

        public void CreateRoleForUser(User user, string role)
        {
            throw new NotImplementedException();
        }

	    public void UpdateUser(User user)
	    {
	        if (user == null)
	            throw new ArgumentNullException("user");

	        UpdateResult result = _userRepository.Update(user);
	        switch (result)
	        {
	            case UpdateResult.ItemNotExists:
	                throw new UserNotExistsException(user.Id);
	            case UpdateResult.Stalled:
	                throw new StaleUserException(user.Id);
	        }

	    }

	    public ResetPasswordRequest GetActivePasswordResetRequestByToken(string token)
        {
            return _resetPasswdRequestsRepository.GetActiveRequestByToken(token);
        }

        public ResetPasswordRequest CreatePasswordResetRequestForUser(User user, DateTime expiration)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            ResetPasswordRequest request = new ResetPasswordRequest
            {
                Token = Guid.NewGuid().ToString(),
                User = user,
                Expires = expiration
            };

            return _resetPasswdRequestsRepository.Insert(request);
        }

        public void DeleteResetPasswordRequest(ResetPasswordRequest request)
        {
            _resetPasswdRequestsRepository.DeleteRequest(request);
        }
    }
}
